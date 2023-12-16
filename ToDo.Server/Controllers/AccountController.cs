using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Todo.Server.Models;
using Todo.Server.Repositories;
using Todo.Server.Services;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController
        : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AccountRepository _accountRepository;
        private readonly PasswordHasher _passwordHasher;

        public AccountController(ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor,
            AccountRepository accountRepository, PasswordHasher passwordHasher)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [Route("Login", Name = "Login")]
        public async Task<ActionResult> Login(string userName, string password)
        {
            var user = await _accountRepository.GetUser(userName);
            if (user is NullUser)
            {
                return NotFound();
            }

            if (!_passwordHasher.VerifyPassword(user.Password, password))
            {
                return BadRequest();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(ClaimTypes.Name, user.Username)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
           
            await _httpContextAccessor.HttpContext!.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost]
        [Route("Register", Name = "Register")]
        public async Task<ActionResult> Register(RegisterUserViewModel model)
        {
            if (!model.Password.Equals(model.ConfirmPassword))
                return BadRequest();

            var user = await _accountRepository.GetUser(model.UserName);
            if (user is not NullUser)
            {
                return BadRequest();
            }

            var hashedPassword = _passwordHasher.Hash(model.Password);
            await _accountRepository.CreateUser(Guid.NewGuid(), model.UserName, hashedPassword);

            return Ok();
        }
    }
}
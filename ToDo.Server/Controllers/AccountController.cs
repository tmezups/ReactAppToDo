using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Server.Extensions;
using Todo.Server.Models;
using Todo.Server.Repositories;
using Todo.Server.Services;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class AccountController
        : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AccountRepository _accountRepository;
        private readonly PasswordHasher _passwordHasher;

        public AccountController(ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor,
            AccountRepository accountRepository, PasswordHasher passwordHasher)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserViewModel>> Login(LoginViewModel model)
        {
            var user = await _accountRepository.GetUser(model.UserName);
            if (user is NullUser)
            {
                LogUserNotFound(model.UserName);
                return NotFound();
            }

            if (!_passwordHasher.VerifyPassword(user.Password, model.Password))
            {
                LogIncorrectPassword(model.UserName);
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

            return Ok(new UserViewModel(user.Username));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Guid>> Register(RegisterUserViewModel model)
        {
            if (!model.Password.Equals(model.ConfirmPassword))
                return BadRequest();

            var user = await _accountRepository.GetUser(model.UserName);
            if (user is not NullUser)
            {
                LogUserAlreadyExists(model.UserName);
                return BadRequest();
            }

            var hashedPassword = _passwordHasher.Hash(model.Password);
            await _accountRepository.CreateUser(Guid.NewGuid(), model.UserName, hashedPassword);
            user = await _accountRepository.GetUser(model.UserName);
            
            return Created("Register", user.Id);
        }
        

        [HttpGet("User")]
        [Authorize]
        public ActionResult<string> GetUser()
        {
            var userName = User.GetUserName();
            if (userName is null)
            {
                return BadRequest();
            }

            return Ok(new UserViewModel(userName));
        }
        
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync();

            return NoContent();
        }
        
        [LoggerMessage(0, LogLevel.Information, "Incorrect password supplied for user: {UserName}")]
        partial void LogIncorrectPassword(string userName);
        
        [LoggerMessage(0, LogLevel.Information, "User not found: {UserName}")]
        partial void LogUserNotFound(string userName);
        
        [LoggerMessage(0, LogLevel.Information, "User trying to register already exists: {UserName}")]
        partial void LogUserAlreadyExists(string userName);
    }
    
    public record UserViewModel(string UserName);
}
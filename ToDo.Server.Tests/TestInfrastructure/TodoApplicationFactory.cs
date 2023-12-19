using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace ToDo.Server.Tests.TestInfrastructure;

public class TodoApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DatabaseFixture _databaseContainer;

    public TodoApplicationFactory()
    {
        _databaseContainer = new DatabaseFixture();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            config =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["ConnectionStrings:Todo"] = _databaseContainer.DatabaseContainer.GetConnectionString()
                    }!
                );
            }
        );
        
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>(
                    "TestScheme", options => { });
        });
        
        builder.ConfigureServices(
            services =>
            {
                services.AddSingleton<UserAccountTestHelper>();
                services.AddSingleton<ToDoTestHelper>();
            });
       
        builder.UseEnvironment("Test");
    }
    
        

    public async Task InitializeAsync()
    {
        await _databaseContainer.DatabaseContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _databaseContainer.DatabaseContainer.StopAsync();
    }
}

public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public static readonly Guid MockUserAccountId = Guid.Parse("8BFE87B9-4F3E-4485-B1B7-DA1AA28486E7");

    public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, MockUserAccountId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ToDo.Server.Tests.TestInfrastructure;

public class UserAccountApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly DatabaseFixture _databaseContainer;
    
    public UserAccountApplicationFactory()
    {
        _databaseContainer = new DatabaseFixture();
    }
    
    public event Action<IServiceCollection>? ConfiguringServices;
    
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
            ConfiguringServices?.Invoke(services);
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
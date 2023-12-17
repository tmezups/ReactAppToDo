using Dapper;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace ToDo.Server.Tests.TestInfrastructure;

public class TodoApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _databaseContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Password1234!")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools/bin/sqlcmd","-Q", "Select 1;"))
        .WithBindMount(Path.GetFullPath("Database"), "/scripts/")
        .WithCommand("/bin/bash", "-c", "/scripts/init.sh")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            config =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["ConnectionStrings:Todo"] = _databaseContainer.GetConnectionString()
                    }!
                );
            }
        );
        
        builder.ConfigureServices(
            services =>
            {
                services.AddSingleton<UserAccountTestHelper>();
            });
       
        builder.UseEnvironment("Test");
    }
    
        

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _databaseContainer.StopAsync();
    }
}

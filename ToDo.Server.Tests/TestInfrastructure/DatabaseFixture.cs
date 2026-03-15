using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;

namespace ToDo.Server.Tests.TestInfrastructure;

public class DatabaseFixture
{
    public readonly MsSqlContainer DatabaseContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Password1234!")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd","-C", "-Q", "Select 1;"))
        .WithBindMount(Path.GetFullPath("Database"), "/scripts/")
        .WithCommand("/bin/bash", "-c", "/scripts/init.sh")
        .Build();

}
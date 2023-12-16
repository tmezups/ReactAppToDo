using Dapper;
using Todo.Server.Models;
using Todo.Server.Options;
using Todo.Server.Services;

namespace Todo.Server.Repositories;

public class AccountRepository
{
    private readonly DapperConnectionProvider _dataSource;

    public AccountRepository(DapperConnectionProvider dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<User> GetUser(string userName)
    {
        using var connection = _dataSource.CreateConnection();
        connection.Open();
        var user = await connection.QueryAsync<User?>(
            "SELECT UserAccountId as Id, Username, Password FROM UserAccount WHERE Username = @userName",
            new { userName });
        return user.FirstOrDefault() ?? new NullUser();
    }

    public async Task CreateUser(Guid userAccountId, string userName, string hashedPassword)
    {
        using var connection = _dataSource.CreateConnection();
        connection.Open();
        await connection.ExecuteAsync(
            "insert into UserAccount(UserAccountId, Username, Password) values (@userAccountId, @userName, @password)",
            new { userAccountId, userName, password = hashedPassword });
    }
}
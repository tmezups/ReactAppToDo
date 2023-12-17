using Dapper;
using Todo.Server.Services;

namespace ToDo.Server.Tests.TestInfrastructure;

public class UserAccountTestHelper
{
    private readonly DapperConnectionProvider _connection;
    private readonly PasswordHasher _passwordHasher;

    public UserAccountTestHelper(DapperConnectionProvider dapperConnectionProvider, PasswordHasher passwordHasher)
    {
        _connection = dapperConnectionProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<TestUserAccount>> GetUserAccounts()
    {
        using var connection = _connection.CreateConnection();
        return (await connection.QueryAsync<TestUserAccount>(
            """
            select UserAccountId, Username, Password, Createdon from UserAccount
            """)).ToList();
    }
    
    
    public async Task AddUserAccount(TestUserAccount userAccount)
    {
        using var connection = _connection.CreateConnection();
        await connection.ExecuteAsync(
            """
            insert into UserAccount(UserAccountID, Username, Password)
            values(@UserAccountID, @Username, @Password)
            """,
            new
            {
                UserAccountID = userAccount.UserAccountId,
                userAccount.Username,
                Password = _passwordHasher.Hash(userAccount.Password),
                userAccount.CreatedOn
            });
    }
    
    public async Task DeleteUserAccount(string Username)
    {
        using var connection = _connection.CreateConnection();
        await connection.ExecuteAsync(
            """
            delete from UserAccount
            where Username = @Username
            """,
            new
            {
                Username
            });
    }
    
}

public record TestUserAccount(Guid UserAccountId, string Username, string Password, DateTime CreatedOn);
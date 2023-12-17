using Dapper;
using ToDo.Server.Controllers;
using Todo.Server.Services;

namespace ToDo.Server.Tests.TestInfrastructure;

public class ToDoTestHelper
{
    private readonly DapperConnectionProvider _connection;
    private readonly PasswordHasher _passwordHasher;

    public ToDoTestHelper(DapperConnectionProvider dapperConnectionProvider, PasswordHasher passwordHasher)
    {
        _connection = dapperConnectionProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<TestToDoItem?> GetToDoItem(string title)
    {
        using var connection = _connection.CreateConnection();
        return await connection.QueryFirstAsync<TestToDoItem>(
            """
            select ToDoId, UserAccountId, Title, IsDone, CreatedOn, UpdatedOn from ToDo
            where Title = @Title
            """,
            new
            {
                Title = title
            });
    }
    
    public async Task<TestToDoItem?> GetToDoItemById(Guid todoId)
    {
        using var connection = _connection.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TestToDoItem>(
            """
            select ToDoId, UserAccountId, Title, IsDone, CreatedOn, UpdatedOn from ToDo
            where ToDoId = @ToDoId
            """,
            new
            {
                ToDoId = todoId
            });
    }

    public async Task DeleteTodoItem(Guid todoId)
    {
        using var connection = _connection.CreateConnection();
        await connection.ExecuteAsync(
            """
            delete from ToDo
            where ToDoId = @ToDoId
            """,
            new
            {
                ToDoId = todoId
            });
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

    public async Task AddToDoItem(TestToDoItem requestData)
    { 
        using var connection = _connection.CreateConnection();
        await connection.ExecuteAsync(
            """
            insert into ToDo (ToDoId, Title, IsDone, UserAccountId)
            values(@ToDoId, @Title, @IsDone, @UserAccountId)
            """,
            new
            {
                requestData.ToDoId,
                requestData.Title,
                requestData.IsDone,
                requestData.UserAccountId
            });
    }
}

public record TestToDoItem(Guid ToDoId, Guid UserAccountId, string Title, bool IsDone, DateTime CreatedOn, DateTime UpdatedOn);
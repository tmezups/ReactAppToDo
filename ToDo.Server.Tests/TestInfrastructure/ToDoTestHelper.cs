using Dapper;
using Todo.Server.Services;

namespace ToDo.Server.Tests.TestInfrastructure;

public class ToDoTestHelper
{
    private readonly IDatabaseConnectionProvider _connection;
    private readonly PasswordHasher _passwordHasher;

    public ToDoTestHelper(IDatabaseConnectionProvider connectionProvider, PasswordHasher passwordHasher)
    {
        _connection = connectionProvider;
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
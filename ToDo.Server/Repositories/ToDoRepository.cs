using Dapper;
using Todo.Server.Models;
using Todo.Server.Services;

namespace Todo.Server.Repositories;

public class ToDoRepository
{
    
    private readonly DapperConnectionProvider _dataSource;
    
    public ToDoRepository(DapperConnectionProvider dataSource)
    {
        _dataSource = dataSource;
    }
    
    public async Task<IEnumerable<ToDoItem>> GetAllToDoItems(Guid userAccountId)
    {
        using var connection = _dataSource.CreateConnection();
        connection.Open();
        return await connection.QueryAsync<ToDoItem>(
            @"
            SELECT Title, TodoId, IsDone, CreatedOn, UpdatedOn FROM Todo
            WHERE UserAccountId = @UserAccountId
            ",
            new
            {
                UserAccountId = userAccountId
            });
    }   

    public async Task AddToDoItem(ToDoItem toDoItem)
    {
       using var connection = _dataSource.CreateConnection();
        await connection.ExecuteAsync(
            @"
            insert into ToDo (TodoId, Title, IsDone, UserAccountId)
            values(@ToDoId, @Title, @IsDone, @UserAccountId)
            ",
            new
            {
                toDoItem.ToDoId,
                toDoItem.Title,
                toDoItem.IsDone,
                toDoItem.UserAccountId
            }); 
    }

    public async Task UpdateToDo(ToDoItem todoItem)
    {
        using var connection = _dataSource.CreateConnection();
        await connection.ExecuteAsync(
            @"
        UPDATE ToDo 
        SET Title = @Title, IsDone = @IsDone, UserAccountId = @UserAccountId, UpdatedOn = GETDATE()
        WHERE ToDoId = @ToDoId
        ",
            new
            {
                todoItem.ToDoId,
                todoItem.Title,
                todoItem.IsDone,
                todoItem.UserAccountId
            }); 
    }

    public async Task<ToDoItem> GetToDoItem(Guid toDoId)
    {
        using var connection = _dataSource.CreateConnection();
        var todo = await connection.QueryFirstOrDefaultAsync<ToDoItem>(
            @"
            select ToDoId, UserAccountId, Title, IsDone, CreatedOn, UpdatedOn from ToDo
            where ToDoId = @ToDoId
            ",
            new
            {
                ToDoId = toDoId
            });
        
        return todo ?? new NullableToDoItem();
    }

    public async Task DeleteToDo(ToDoItem todoItem)
    {
        using var connection = _dataSource.CreateConnection();
        await connection.ExecuteAsync(
            @"
            delete from ToDo
            where ToDoId = @ToDoId
            ",
            new
            {
                ToDoId = todoItem.ToDoId
            });
    }
    
    public async Task<IEnumerable<ToDoItem>> SearchToDo(DateTime startDate, DateTime endDate)
    {
        using var connection = _dataSource.CreateConnection();
        return await connection.QueryAsync<ToDoItem>(
            @"
               select ToDoId, UserAccountId, Title, IsDone, CreatedOn, UpdatedOn from ToDo
               where CreatedOn >= @startDate and CreatedOn <= @endDate
            ",
            new
            {
                startDate,
                endDate
            });
    }
    
}
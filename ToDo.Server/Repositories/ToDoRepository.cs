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
    
    public async Task<IEnumerable<ToDoItem>> GetAllToDoItems()
    {
        using var connection = _dataSource.CreateConnection();
        connection.Open();
        return await connection.QueryAsync<ToDoItem>("SELECT Title, Id, IsDone, CreatedOn, UpdatedOn FROM Todo");
    }
}
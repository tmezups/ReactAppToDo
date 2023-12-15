using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController(ILogger<ToDoController> logger, ToDoRepository toDoRepository)
        : ControllerBase
    {
//TODO update logger to partial
//TODO add nlog config

        private readonly ILogger<ToDoController> _logger = logger;

        [HttpGet(Name = "GetToDoItems")]
        public async Task<IEnumerable<Todo.Server.Controllers.ToDo>> GetToDoCollection()
        {
            return  (await  toDoRepository.GetAllToDoItems()).ToList();
        }
    }
}

public class ToDoRepository
{
    
    private readonly DapperConnectionProvider _dataSource;
    
    public ToDoRepository(DapperConnectionProvider dataSource)
    {
        _dataSource = dataSource;
    }
    
    public async Task<IEnumerable<Todo.Server.Controllers.ToDo>> GetAllToDoItems()
    {
        using var connection = _dataSource.CreateConnection();
        connection.Open();
        return await connection.QueryAsync<Todo.Server.Controllers.ToDo>("SELECT Title, Id, IsDone, CreatedOn, UpdatedOn FROM Todo");
    }
}


public class DapperConnectionProvider
{

    private readonly ConnectionStrings _connectionString;

    public DapperConnectionProvider(IOptions<ConnectionStrings> connectionStrings)
    {
        _connectionString = connectionStrings.Value;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString.GetConnectionStringValue());
}

public record ConnectionStrings
{
    public string ToDo { get; set; } = "";

    public string GetConnectionStringValue()
    {
        return ToDo;
    }
}
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Todo.Server.Configuration;

namespace Todo.Server.Services;

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
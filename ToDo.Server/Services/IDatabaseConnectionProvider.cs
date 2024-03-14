using System.Data;

namespace Todo.Server.Services;

public interface IDatabaseConnectionProvider
{
    IDbConnection CreateConnection();
}
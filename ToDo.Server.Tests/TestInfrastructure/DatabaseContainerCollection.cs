using Xunit;

namespace ToDo.Server.Tests.TestInfrastructure;


[CollectionDefinition(nameof(DatabaseContainerCollection))]
public class DatabaseContainerCollection : ICollectionFixture<TodoApplicationFactory>
{
}
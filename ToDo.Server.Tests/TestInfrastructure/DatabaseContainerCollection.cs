using Xunit;

namespace ToDo.Server.Tests.TestInfrastructure;


[CollectionDefinition(nameof(TodoApplicationFactoryCollection))]
public class TodoApplicationFactoryCollection : ICollectionFixture<TodoApplicationFactory>
{
}


[CollectionDefinition(nameof(UserAccountApplicationFactoryCollection))]
public class UserAccountApplicationFactoryCollection : ICollectionFixture<UserAccountApplicationFactory>
{
}
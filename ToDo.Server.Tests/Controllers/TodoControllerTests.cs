using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using ToDo.Server.Controllers;
using Todo.Server.Repositories;
using ToDo.Server.Tests.TestInfrastructure;
using Xunit;
using Xunit.Abstractions;

namespace ToDo.Server.Tests.Controllers;

[Collection(nameof(TodoApplicationFactoryCollection))]
public class TodoControllerTests
{
    private readonly TodoApplicationFactory _fixture;
    private readonly ITestOutputHelper _output;
    private readonly Lazy<HttpClient> _httpClient;

    public TodoControllerTests(TodoApplicationFactory fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _httpClient = new Lazy<HttpClient>(() => {
            var client = fixture.CreateClient();
            return client;
        });
    }
    
    
    [Fact]
    public async Task ShouldCreateToDoItem()
    { 
        var requestData = new CreateToDoItemViewModel("first to do", false);
        var response = await _httpClient.Value.PostAsJsonAsync("/ToDo/Create", requestData);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        (await testHelper.GetToDoItem(requestData.Title)).ShouldNotBeNull();
    }
    
    
    [Fact]
    public async Task ShouldUpdateExistingToDoItem()
    {
        var requestData = new TestToDoItem(Guid.Parse("BCFD2D73-3E00-4FD6-A651-1FE4F8DD3352"),
            MockAuthenticationHandler.MockUserAccountId, 
            "updated title", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.AddToDoItem(requestData);
        
        var response = await _httpClient.Value.PutAsJsonAsync("/ToDo/Update", requestData);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var todo = (await testHelper.GetToDoItemById(requestData.ToDoId))!;
        todo.IsDone.ShouldBeTrue();
        todo.Title.ShouldBe(requestData.Title);
    }
    
    
    [Fact]
    public async Task ShouldDeleteExistingToDoItem()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "delete me", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.AddToDoItem(requestData);
        
        var response = await _httpClient.Value.DeleteAsync($"/ToDo/Delete/{requestData.ToDoId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        var todo = (await testHelper.GetToDoItemById(requestData.ToDoId))!;
        todo.ShouldBeNull();
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenTryingToDeleteTodoThatDoesNotExist()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "try and delete me", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        
        var response = await _httpClient.Value.DeleteAsync($"/ToDo/Delete/{requestData.ToDoId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldNotAllowAnotherUserToDeleteAnotherUsersToDo()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            Guid.NewGuid(), //just a random user
            "delete me", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.AddToDoItem(requestData);
        
        var response = await _httpClient.Value.DeleteAsync($"/ToDo/Delete/{requestData.ToDoId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenTryingToUpdateToDoThatDoesNotExist()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "update me", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        
        var response = await _httpClient.Value.PutAsJsonAsync("/ToDo/Update", requestData);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldNotAllowAnotherUserToUpdateAnotherUsersToDo()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            Guid.NewGuid(), //just a random user
            "update me", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.AddToDoItem(requestData);
        
        var response = await _httpClient.Value.PutAsJsonAsync("/ToDo/Update", requestData);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    
    [Fact]
    public async Task ShouldReturnCompleteToDoListForLoggedInUser()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "my todo collection", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var otherUserRequestData = new TestToDoItem(Guid.NewGuid(),
            Guid.NewGuid(), 
            "other users todo", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.DeleteTodoItem(otherUserRequestData.ToDoId);
        //Make sure the todo item exists for the logged in user
        await testHelper.AddToDoItem(requestData);
        await testHelper.AddToDoItem(otherUserRequestData);
        
        var response = await _httpClient.Value.GetAsync("/ToDo/GetAll");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var todos = (await response.Content.ReadFromJsonAsync<IEnumerable<ToDoItem>>() ?? Array.Empty<ToDoItem>()).ToList();
        todos.Count.ShouldBeGreaterThanOrEqualTo(1);
        var todo = todos.First(t => t.ToDoId == requestData.ToDoId);
        todo.Title.ShouldBe(requestData.Title);
        todo.ToDoId.ShouldBe(requestData.ToDoId);
        todo.IsDone.ShouldBe(requestData.IsDone);

    }
    
    
    [Fact]
    public async Task ShouldReturnSingleToDoItem()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "my todo", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var otherUserRequestData = new TestToDoItem(Guid.NewGuid(),
            Guid.NewGuid(), 
            "other users todo", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.DeleteTodoItem(otherUserRequestData.ToDoId);
        //Make sure the todo item exists for the logged in user
        await testHelper.AddToDoItem(requestData);
        await testHelper.AddToDoItem(otherUserRequestData);
        
        var response = await _httpClient.Value.GetAsync($"/ToDo/{requestData.ToDoId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var todo = await response.Content.ReadFromJsonAsync<ToDoItem>();
        todo!.Title.ShouldBe(requestData.Title);
        todo.ToDoId.ShouldBe(requestData.ToDoId);
        todo.IsDone.ShouldBe(requestData.IsDone);

    }
    
    
    [Fact]
    public async Task ShouldNotAllowAnotherUserToGetAnotherUsersTodo()
    {
        var requestData = new TestToDoItem(Guid.NewGuid(),
            MockAuthenticationHandler.MockUserAccountId, 
            "my todo", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var otherUserRequestData = new TestToDoItem(Guid.NewGuid(),
            Guid.NewGuid(), 
            "other users todo", 
            true, 
            DateTime.MinValue, 
            DateTime.MinValue );
        var testHelper = _fixture.Services.GetRequiredService<ToDoTestHelper>();
        await testHelper.DeleteTodoItem(requestData.ToDoId);
        await testHelper.DeleteTodoItem(otherUserRequestData.ToDoId);
        //Make sure the todo item exists for the logged in user
        await testHelper.AddToDoItem(requestData);
        await testHelper.AddToDoItem(otherUserRequestData);
        
        var response = await _httpClient.Value.GetAsync($"/ToDo/{otherUserRequestData.ToDoId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine(responseContent);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);

    }
    
    
    [Fact]
    public async Task ShouldReturnNotFoundWhenRequestedToDoDoesNotExist()
    {
        var response = await _httpClient.Value.GetAsync($"/ToDo/Get/{Guid.NewGuid()}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

}



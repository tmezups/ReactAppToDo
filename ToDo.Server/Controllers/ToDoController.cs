using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Server.Extensions;
using Todo.Server.Repositories;

namespace ToDo.Server.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ToDoController(ILogger<ToDoController> logger, ToDoRepository toDoRepository)
    : ControllerBase
{
    private readonly ILogger<ToDoController> _logger = logger;

    [HttpGet("GetAll")]
    public async Task<IEnumerable<ToDoItem>> GetAll()
    {
        return (await toDoRepository.GetAllToDoItems(User.GetUserAccountId())).ToList();
    }

    [HttpGet("Get/{todoId}")]
    public async Task<ActionResult<ToDoItem>> Get(Guid toDoId)
    {
        var todoItem = await toDoRepository.GetToDoItem(toDoId);

        if (todoItem is NullableToDoItem)
            return NotFound();
        
        if (!IsCorrectUserForTodo(todoItem))
            return Forbid();
        
        return todoItem;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<ToDoItem>> Create(CreateToDoItemViewModel toDoItemViewModel)
    {
        var todoItem = new ToDoItem
        {
            ToDoId = Guid.NewGuid(),
            UserAccountId = User.GetUserAccountId(),
            Title = toDoItemViewModel.Title,
            IsDone = toDoItemViewModel.IsDone,
        };
        await toDoRepository.AddToDoItem(todoItem);
        return CreatedAtAction(nameof(Get), new { id = todoItem.ToDoId }, todoItem);
    }


    [HttpPut("Update")]
    public async Task<ActionResult<ToDoItem>> Update(ToDoItemViewModel toDoItemViewModel)
    {
        var todoItem = await toDoRepository.GetToDoItem(toDoItemViewModel.ToDoId);
       
        if (!IsCorrectUserForTodo(todoItem))
            return Forbid();
        
        todoItem.Title = toDoItemViewModel.Title;
        todoItem.IsDone = toDoItemViewModel.IsDone;
        await toDoRepository.UpdateToDo(todoItem);
        return NoContent();
    }
    
    [HttpDelete("Delete/{todoId}")]
    public async Task<ActionResult> Delete(Guid todoId)
    {
        var todoItem = await toDoRepository.GetToDoItem(todoId);
        if (todoItem is NullableToDoItem)
            return NotFound();

        if (!IsCorrectUserForTodo(todoItem))
            return Forbid();
        
        await toDoRepository.DeleteToDo(todoItem);
        
        return NoContent();
    }
    
    
    private bool IsCorrectUserForTodo(ToDoItem todoItem)
    {
        return todoItem.UserAccountId.Equals(User.GetUserAccountId());
    }
}

public record CreateToDoItemViewModel(string Title, bool IsDone);

public readonly record struct ToDoItemViewModel(Guid ToDoId, string Title, bool IsDone);
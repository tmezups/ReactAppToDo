using Microsoft.AspNetCore.Mvc;
using Todo.Server.Models;
using Todo.Server.Repositories;

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
        public async Task<IEnumerable<ToDoItem>> GetToDoCollection()
        {
            return  (await  toDoRepository.GetAllToDoItems()).ToList();
        }
    }
}
namespace Todo.Server.Models;

public record NullableToDoItem() : ToDoItem(Guid.Empty, Guid.Empty) {}
namespace Todo.Server.Models;

public readonly record struct ToDoItem(string Title, Guid Id, bool IsDone, DateTime CreatedOn, DateTime UpdatedOn);
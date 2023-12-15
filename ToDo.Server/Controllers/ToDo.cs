namespace Todo.Server.Controllers;

public readonly record struct ToDo(string Title, Guid Id, bool IsDone, DateTime CreatedOn, DateTime UpdatedOn);
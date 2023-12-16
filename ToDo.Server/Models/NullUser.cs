namespace Todo.Server.Models;

public record NullUser() : User(Guid.Empty, string.Empty, string.Empty);
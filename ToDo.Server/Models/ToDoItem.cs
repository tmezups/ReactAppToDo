namespace Todo.Server.Models;

public record ToDoItem(Guid ToDoId, Guid UserAccountId)
{
    public ToDoItem() : this(Guid.Empty, Guid.Empty)
    {
    }
    public bool IsDone { get; set; }
    public string Title { get; set; } = "";

}


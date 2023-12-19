namespace Todo.Server.Models;

public class ToDoItem
{
    public Guid  ToDoId { get; set; }
    public Guid UserAccountId { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
}
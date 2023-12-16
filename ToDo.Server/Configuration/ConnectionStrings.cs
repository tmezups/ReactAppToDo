namespace Todo.Server.Options;

public record ConnectionStrings
{
    public string ToDo { get; set; } = "";

    public string GetConnectionStringValue()
    {
        return ToDo;
    }
}
namespace Todo.Server.Configuration;

public record ConnectionStrings
{
    public string ToDo { get; set; } = "";

    public string GetConnectionStringValue()
    {
        return ToDo;
    }
}
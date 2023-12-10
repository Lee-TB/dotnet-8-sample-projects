namespace TodoApi;

public class TodoDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public TodoDTO(Todo todo)
    {
        Id = todo.Id;
        Name = todo.Name;
        IsComplete = todo.IsComplete;
    }
    public TodoDTO() { }
}
namespace TodoApi.Entities;

public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }

    public TodoDTO AsDTO()
    {
        return new TodoDTO()
        {
            Id = this.Id,
            Name = this.Name,
            IsComplete = this.IsComplete
        };
    }
}
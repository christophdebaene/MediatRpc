namespace Sample.Domain;
public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoPriority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public List<Guid> Documents { get; set; } = new();
}

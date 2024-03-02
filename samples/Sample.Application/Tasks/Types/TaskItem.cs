namespace Sample.Application.Tasks.Types;

public class TaskItem(Guid id, string title)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public DateTime? DueDate { get; set; } = DateTime.UtcNow.AddDays(1);
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public bool IsCompleted { get; set; } = false;
    public List<Guid> Documents { get; set; } = [];
}

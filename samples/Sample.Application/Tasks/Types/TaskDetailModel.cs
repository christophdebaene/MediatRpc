namespace Sample.Application.Tasks.Types;

public record TaskDetailModel(Guid Id, string Title, TaskPriority Priority, bool IsCompleted)
{
}
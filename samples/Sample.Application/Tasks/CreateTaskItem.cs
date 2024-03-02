using MediatR;
using MediatRpc;
using Sample.Application.Tasks.Types;

namespace Sample.Application.Tasks;

[Command]
public record CreateTaskItem(Guid TaskId, string Title) : IRequest
{
}
public class CreateTaskItemHandler(ApplicationContext context) : IRequestHandler<CreateTaskItem>
{
    public async Task Handle(CreateTaskItem command, CancellationToken cancellationToken)
    {
        var task = new TaskItem(command.TaskId, command.Title);
        await context.Tasks.AddAsync(task, cancellationToken);
    }
}

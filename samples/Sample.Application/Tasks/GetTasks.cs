using MediatR;
using MediatRpc;
using Microsoft.EntityFrameworkCore;
using Sample.Application.Tasks.Types;

namespace Sample.Application.Tasks;

[Query]
public record GetTasks : IRequest<IReadOnlyList<TaskHeader>>
{
}
public record TaskHeader(Guid Id, string Title, TaskPriority Priority);

public class GetTasksHandler(ApplicationContext context) : IRequestHandler<GetTasks, IReadOnlyList<TaskHeader>>
{
    public async Task<IReadOnlyList<TaskHeader>> Handle(GetTasks query, CancellationToken cancellationToken)
    {
        return await context.Tasks
           .AsNoTracking()
           .Select(x => new TaskHeader(x.Id, x.Title, x.Priority))
           .ToListAsync(cancellationToken);
    }
}
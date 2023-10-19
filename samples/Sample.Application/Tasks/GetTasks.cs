using MediatR;
using MediatRpc;
using Microsoft.EntityFrameworkCore;
using Sample.Domain;

namespace Sample.Application.Tasks;

[Query]
[RequestContract("MyApp", "Tasks", "GetTasks")]
public record GetTasks : IRequest<IReadOnlyList<TodoDetailModel>>
{
}
public class GetTasksHandler : IRequestHandler<GetTasks, IReadOnlyList<TodoDetailModel>>
{
    private readonly TodoContext _context;
    public GetTasksHandler(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IReadOnlyList<TodoDetailModel>> Handle(GetTasks query, CancellationToken cancellationToken)
    {
        return await _context.Set<Todo>()
           .AsNoTracking()
           .Select(x => new TodoDetailModel
           {
               Id = x.Id,
               Title = x.Title,
               Priority = x.Priority.ToString()
           })
           .ToListAsync(cancellationToken);
    }
}
public class TodoDetailModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Priority { get; set; }
}


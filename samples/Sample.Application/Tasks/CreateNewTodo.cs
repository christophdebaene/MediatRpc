using MediatR;
using MediatRpc;
using Sample.Domain;

namespace Sample.Application.Tasks;

[Command]
public record CreateNewTodo : IRequest
{
    public Guid TodoId { get; init; }
    public string Title { get; init; }
}

public class CreateNewTaskHandler : IRequestHandler<CreateNewTodo>
{
    private readonly TodoContext _context;
    public CreateNewTaskHandler(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task Handle(CreateNewTodo request, CancellationToken cancellationToken)
    {
        var task = new Todo
        {
            Id = request.TodoId,
            Title = request.Title,
            Priority = TodoPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        await _context.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync();

        return;
    }
}

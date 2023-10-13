using MediatR;
using MediatRpc;
using Sample.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sample.Application.Tasks;

[Query]
public record GetTodoDetail : IRequest<JsonElement>
{
    public Guid TodoId { get; init; }
}
public class GetTodoDetailHandler : IRequestHandler<GetTodoDetail, JsonElement>
{
    private readonly TodoContext _context;    
    public GetTodoDetailHandler(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));        
    }
    public async Task<JsonElement> Handle(GetTodoDetail request, CancellationToken cancellationToken)
    {
        var todo = await _context.Set<Todo>().FindAsync(request.TodoId);
        var jsonElement = JsonSerializer.SerializeToElement(todo, new JsonSerializerOptions
        {
            Converters ={
                new JsonStringEnumConverter()
        }});

        return jsonElement;
    }
}
using MediatR;
using MediatRpc;

namespace Sample.Application.Calculator;

[Query]
public class Add : IRequest<long>
{
    public long X { get; set; }
    public long Y { get; set; }
}
public class AddHandler : IRequestHandler<Add, long>
{
    public Task<long> Handle(Add request, CancellationToken cancellationToken) => Task.FromResult(request.X + request.Y);
}

using MediatR;

namespace Sample.Application.Calculator;
public class Substract : IRequest<long>
{
    public long X { get; set; }
    public long Y { get; set; }
}
public class SubstractHandler : IRequestHandler<Add, long>
{
    public Task<long> Handle(Add request, CancellationToken cancellationToken) => Task.FromResult(request.X - request.Y);
}

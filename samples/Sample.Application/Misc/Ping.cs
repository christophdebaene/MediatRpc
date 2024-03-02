using MediatR;
using MediatRpc;

namespace Sample.Application.Misc;

[Query]
public class Ping : IRequest<string>
{
}
public class PingHandler : IRequestHandler<Ping, string>
{
    public Task<string> Handle(Ping request, CancellationToken cancellationToken) => Task.FromResult("Pong");
}

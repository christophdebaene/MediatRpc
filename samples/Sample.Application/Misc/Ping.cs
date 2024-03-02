using MediatR;

namespace Sample.Application.Misc;

public class Ping : IRequest<string>
{
}
public class PingHandler : IRequestHandler<Ping, string>
{
    public Task<string> Handle(Ping request, CancellationToken cancellationToken) => Task.FromResult("Pong");
}

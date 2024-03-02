using MediatRpc.DependencyInjection;
using System.Collections.Generic;

namespace MediatRpc.Metadata;

public interface IEndpointProvider
{
    IEnumerable<EndpointInfo> Resolve(MediatRpcConfiguration configuration);
}

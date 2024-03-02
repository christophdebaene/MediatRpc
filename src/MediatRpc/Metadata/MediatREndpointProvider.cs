using MediatRpc.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatRpc.Metadata;

public class MediatREndpointProvider : IEndpointProvider
{
    public IEnumerable<EndpointInfo> Resolve(MediatRpcConfiguration configuration)
    {
        var endpoints = new List<EndpointInfo>();

        foreach (var requestType in configuration.AssembliesToRegister.SelectMany(x => x.GetExportedTypes())
                                    .Where(x => !x.IsAbstract && !x.IsInterface && ImplementsBaseRequest(x)))
        {
            var name = configuration.RequestName(requestType);

            var requestInterface = requestType.GetInterface("IRequest`1");
            if (requestInterface is not null)
            {
                var responseType = requestInterface.GetGenericArguments().Single();
                if (responseType.FullName == "MediatR.Unit")
                {
                    endpoints.Add(new(name, requestType, null));
                }
                else
                {
                    endpoints.Add(new(name, requestType, responseType));
                }
            }
            else
            {
                endpoints.Add(new(name, requestType, null));
            }
        }

        return endpoints;
    }
    static bool ImplementsBaseRequest(Type type) => type.GetInterfaces().Any(x => x.FullName == "MediatR.IBaseRequest");
}

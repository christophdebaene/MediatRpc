using MediatRpc.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatRpc.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRpc(this IServiceCollection services, Action<MediatRpcConfiguration> configuration)
    {
        var serviceConfig = new MediatRpcConfiguration();
        configuration.Invoke(serviceConfig);
        return services.AddMediatRpc(serviceConfig);
    }
    public static IServiceCollection AddMediatRpc(this IServiceCollection services, MediatRpcConfiguration configuration)
    {
        if (!configuration.AssembliesToRegister.Any())
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }

        var requests = new List<RequestInfo>();

        foreach (var requestType in configuration.AssembliesToRegister.SelectMany(x => x.ScanRequestTypes()))
        {
            var handlers = configuration.AssembliesToRegister.SelectMany(x => x.ScanRequestHandlerTypes(requestType));

            if (handlers.Count() > 1)
                throw new Exception($"Multiple handlers for {requestType.FullName}");

            var requestContract = configuration.ContractResolver(requestType);
            
            requests.Add(new RequestInfo(
                requestType,
                handlers.FirstOrDefault(),
                requestContract));
        }

        if (configuration.VerifyRequestType)
        {
            foreach (var request in requests.Where(x => x.Type == RequestType.Unknown))
                throw new System.Exception($"Request {request} is unknown");
        }

        var typeCatalog = new TypeCatalog(requests);        
        return services.AddSingleton(typeCatalog);
    }    
}

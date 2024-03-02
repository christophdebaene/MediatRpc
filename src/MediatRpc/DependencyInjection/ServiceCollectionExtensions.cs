using MediatRpc.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        if (configuration.AssembliesToRegister.Count == 0)
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }

        var endpoints = configuration.EndpointProvider.Resolve(configuration).ToList();

        if (configuration.VerifyRequestType)
        {
            foreach (var request in endpoints.Where(x => RequestTypeAttribute.Get(x.RequestType) == RequestType.Unknown))
                throw new System.Exception($"Request {request} is unknown");
        }

        var typeCatalog = new EndpointCatalog(endpoints);
        return services.AddSingleton(typeCatalog);
    }
}

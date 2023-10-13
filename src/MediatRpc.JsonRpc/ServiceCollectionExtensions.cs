using MediatRpc.Contracts;
using MediatRpc.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MediatRpc.JsonRpc;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJsonRpc(this IServiceCollection services, IRequestContractProvider contractProvider, params Assembly[] assemblies)
    {
        var catalogProvider = new TypeCatalogProvider(new TypeCatalogOptions
        {
            Assemblies = assemblies.ToList(),
            ContractProvider = contractProvider
        });

        var catalog = catalogProvider.Get();

        return services
            .AddSingleton(contractProvider)
            .AddSingleton(catalog);
    }
    public static IServiceCollection UseJsonRpc(this IServiceCollection services, Type exceptionStrategyType)
    {
        services.AddMvcCore()
            .AddApplicationPart(typeof(JsonRpcController).Assembly).AddControllersAsServices();

        if (exceptionStrategyType != null)
            services.AddSingleton(typeof(IExceptionStrategy), exceptionStrategyType);

        return services;
    }
}

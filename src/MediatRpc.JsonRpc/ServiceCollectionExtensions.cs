using Microsoft.Extensions.DependencyInjection;
using System;
namespace MediatRpc.JsonRpc;
public static class ServiceCollectionExtensions
{    
    public static IServiceCollection UseJsonRpc(this IServiceCollection services, Type exceptionStrategyType)
    {
        services.AddMvcCore()
            .AddApplicationPart(typeof(JsonRpcController).Assembly).AddControllersAsServices();

        if (exceptionStrategyType != null)
            services.AddSingleton(typeof(IExceptionStrategy), exceptionStrategyType);

        return services;
    }
}

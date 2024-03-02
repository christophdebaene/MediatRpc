using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MediatRpc.Tools.Redoc;

public static class RedocExtensions
{
    public static IEndpointRouteBuilder UseRedoc(this IEndpointRouteBuilder endpoints, Action<RedocConfiguration>? configuration = null)
    {
        var config = new RedocConfiguration();
        configuration?.Invoke(config);
        return endpoints.UseRedoc(config);
    }
    public static IEndpointRouteBuilder UseRedoc(this IEndpointRouteBuilder endpoints, RedocConfiguration configuration)
    {
        endpoints.MapGet(configuration.RoutePrefix, () => new RedocResult(configuration));
        return endpoints;
    }
}
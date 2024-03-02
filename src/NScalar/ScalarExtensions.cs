using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace NScalar;

public static partial class ScalarExtensions
{
    public static IEndpointRouteBuilder UseScalar(this IEndpointRouteBuilder endpoints, Action<ScalarConfiguration>? configuration = null)
    {
        var config = new ScalarConfiguration();
        configuration?.Invoke(config);
        return endpoints.UseScalar(config);
    }
    public static IEndpointRouteBuilder UseScalar(this IEndpointRouteBuilder endpoints, ScalarConfiguration configuration)
    {
        endpoints.MapGet(configuration.RoutePrefix, () => new ScalarResult(configuration));
        return endpoints;
    }
}
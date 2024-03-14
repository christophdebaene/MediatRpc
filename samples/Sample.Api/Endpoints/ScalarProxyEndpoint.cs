using MediatR;
using MediatRpc.JsonRpc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NScalar;

namespace Sample.Api.Endpoints;
public static class ScalarProxyEndpoint
{
    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapPost("/scalarproxy", async (HttpContext context, ISender sender, ScalarProxyRequest scalarProxy, CancellationToken cancellationToken) =>
        {
            var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
            var error = JsonRpcRequest.TryParse(scalarProxy.Data.ToString(), serializerOptions, out JsonRpcRequest? request);

            if (error is null)
            {

            }
        });

        return app;
    }
}

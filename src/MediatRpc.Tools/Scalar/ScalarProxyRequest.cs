using MediatRpc.JsonRpc;
using MediatRpc.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MediatRpc.Tools.Scalar;

public class ScalarProxyRequest : JsonRpcEndpointResult
{
    public static async ValueTask<ScalarProxyRequest> BindAsync(HttpContext context)
    {
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        var catalog = context.RequestServices.GetRequiredService<EndpointCatalog>();

        var body = await context.Request.ReadFromJsonAsync<ScalarProxyBody>();
        var error = JsonRpcRequest.TryParse(body.Data.ToString(), serializerOptions, out JsonRpcRequest? request);

        if (error is null)
        {
            return new ScalarProxyRequest
            {
                Error = null,
                Endpoint = new JsonRpcEndpoint(catalog[request.Method].Name, catalog[request.Method].RequestType, request)
            };
        }
        else
        {
            return new ScalarProxyRequest
            {
                Error = error,
                Endpoint = null
            };
        }
    }
    internal record ScalarProxyBody(string Method, string Url, JsonElement Data);
}
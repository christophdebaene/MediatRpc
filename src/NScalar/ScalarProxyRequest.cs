using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace NScalar;

public class ScalarProxyRequest
{
    public string Method { get; set; }
    public string Url { get; set; }
    public JsonElement Data { get; set; }
    public static async ValueTask<ScalarProxyRequest?> BindAsync(HttpContext context)
    {
        var request = await context.Request.ReadFromJsonAsync<ScalarProxyRequest>();
        return request;
    }
}

/*
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
*/
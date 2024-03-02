using MediatRpc.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;

public class JsonRpcEndpointResult
{
    public bool IsValid => Error is null;
    public JsonRpcErrorResponse? Error { get; init; }
    public JsonRpcEndpoint? Endpoint { get; init; }
    public static async ValueTask<JsonRpcEndpointResult> BindAsync(HttpContext context)
    {
        var result = await JsonRpcRequestResult.BindAsync(context);
        if (result.Error is not null)
        {
            return new JsonRpcEndpointResult
            {
                Error = result.Error
            };
        }

        var jsonRpcRequest = result.Request!;

        var catalog = context.RequestServices.GetRequiredService<EndpointCatalog>();
        if (!catalog.Exist(jsonRpcRequest.Method))
        {
            return new JsonRpcEndpointResult
            {
                Error = JsonRpcErrorResponse.MethodNotFound(jsonRpcRequest.Id, jsonRpcRequest.Method)
            };
        }

        var requestInfo = catalog[jsonRpcRequest.Method];

        return new JsonRpcEndpointResult
        {
            Error = null,
            Endpoint = new JsonRpcEndpoint(requestInfo.Name, requestInfo.RequestType, jsonRpcRequest)
        };
    }
}

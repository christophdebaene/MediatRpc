using MediatRpc.JsonRpc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;

public class JsonRpcRequestResult
{
    public JsonRpcRequest? Request { get; set; }
    public JsonRpcErrorResponse? Error { get; set; }
    public static async ValueTask<JsonRpcRequestResult> BindAsync(HttpContext context)
    {        
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

        var body = context.Request.HasFormContentType
            ? context.Request.Form["jsonrpc"].FirstOrDefault()
            : await context.Request.GetRawBodyStringAsync();

        var error = JsonRpcRequest.TryParse(body, serializerOptions, out JsonRpcRequest? request);
        return new JsonRpcRequestResult
        {
            Request = request,
            Error = error
        };        
    }
}

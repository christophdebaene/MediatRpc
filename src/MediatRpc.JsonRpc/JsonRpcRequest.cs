using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;
public class JsonRpcRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string JsonRpcVersion { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public JsonElement Params { get; set; }

    public static async ValueTask<JsonRpcRequest> BindAsync(HttpContext context)
    {
        if (!context.Request.Body.CanSeek)
        {
            context.Request.EnableBuffering();
        }
        
        if (context.Request.HasFormContentType)
        {
            var jsonRpc = context.Request.Form["jsonRpc"];
            return JsonSerializer.Deserialize<JsonRpcRequest>(jsonRpc);            
        }
        else
        {
            context.Request.Body.Position = 0;
            return await context.Request.ReadFromJsonAsync<JsonRpcRequest>();
        }
    }
    public object CreateInstance(Type type)
    {
        var request = Params.ValueKind == JsonValueKind.Undefined ? Activator.CreateInstance(type) : JsonSerializer.Deserialize(Params, type);

        if (request is IHaveId iHaveId)
            iHaveId.Id = Id;

        if (request is IHaveParams iHaveParams)
            iHaveParams.Params = Params;

        return request;
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;
public record JsonRpcRequest
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string JsonRpcVersion { get; set; }

    [JsonPropertyName("method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public JsonElement Params { get; set; }

    public static JsonRpcErrorResponse? TryParse(string? content, JsonSerializerOptions settings, out JsonRpcRequest? request)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            request = null;
            return JsonRpcErrorResponse.InvalidRequest(null, "");
        }
        try
        {
            request = JsonSerializer.Deserialize<JsonRpcRequest>(content, settings)!;

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return JsonRpcErrorResponse.InvalidRequest(null, $"Property 'id' is empty or missing");
            }

            if (string.IsNullOrWhiteSpace(request.JsonRpcVersion))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'jsonrpc' is empty or missing");
            }

            if (!request.JsonRpcVersion.Equals("2.0", StringComparison.OrdinalIgnoreCase))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'jsonrpc' must be 2.0");
            }

            if (string.IsNullOrWhiteSpace(request.Method))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'method' is empty or missing");
            }

            if (request.Params.ValueKind != JsonValueKind.Undefined)
            {
                if (!(request.Params.ValueKind == JsonValueKind.Object || request.Params.ValueKind == JsonValueKind.Array))
                {
                    return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'params' must be of type object or array");
                }
            }

            return null;
        }
        catch (Exception exc)
        {
            request = null;
            return JsonRpcErrorResponse.ParseError(null, exc);
        }
    }
    public static async Task<(JsonRpcErrorResponse? Error, JsonRpcRequest? Request)> TryParseAsync(HttpContext context)
    {
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

        if (context.Request.HasFormContentType)
        {
            var content = context.Request.Form["jsonrpc"].FirstOrDefault();
            var error = TryParse(content, serializerOptions, out JsonRpcRequest? request);
            return (error, request);
        }
        else
        {
            if (!context.Request.Body.CanSeek)
            {
                context.Request.EnableBuffering();
            }

            context.Request.Body.Position = 0;

            var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            var error = TryParse(body, serializerOptions, out JsonRpcRequest? request);
            return (error, request);
        }
    }
}

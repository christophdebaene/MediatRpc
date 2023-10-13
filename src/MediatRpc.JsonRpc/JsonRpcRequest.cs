using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    public static JsonRpcRequest Create(string json)
    {
        return JsonSerializer.Deserialize<JsonRpcRequest>(json);
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

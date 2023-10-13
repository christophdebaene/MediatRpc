using System.Text.Json.Serialization;

namespace MediatRpc.JsonRpc;
public class JsonRpcResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string JsonRpcVersion { get; private set; } = "2.0";

    [JsonPropertyName("result")]
    public object Result { get; set; }
}
public class JsonRpcErrorResponse : JsonRpcResponse
{
    [JsonPropertyName("error")]
    public JsonRpcError Error { get; set; }
}

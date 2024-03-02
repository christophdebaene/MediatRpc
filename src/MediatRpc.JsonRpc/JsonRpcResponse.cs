using System.Text.Json.Serialization;

namespace MediatRpc.JsonRpc;
public class JsonRpcResponse
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string JsonRpcVersion { get; private set; } = "2.0";

    [JsonPropertyName("result")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public object? Result { get; set; }
}

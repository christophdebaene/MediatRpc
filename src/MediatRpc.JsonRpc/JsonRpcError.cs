using System.Text.Json.Serialization;

namespace MediatRpc.JsonRpc;
public record JsonRpcError
{
    [JsonPropertyName("code")]
    [JsonPropertyOrder(0)]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    [JsonPropertyOrder(1)]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    [JsonPropertyOrder(2)]
    public object? Data { get; set; }
}

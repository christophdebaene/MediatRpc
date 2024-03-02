using System.Text.Json.Serialization;

namespace MediatRpc.Tools.Scalar;

public class ScalarProxyResponse
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("headers")]
    public ScalarProxyHeader Headers { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; }
}
public record ScalarProxyHeader
{
    [JsonPropertyName("content-type")]
    public string ContentType { get; set; }
}

using NJsonSchema;

namespace MediatRpc.JsonRpc.OpenApi;
public class JsonRpcExtension
{
    public bool isFileRequest { get; set; }
}

public static class JsonRpcExtensions
{
    public static JsonRpcExtension GetJsonRpcExtension(this IJsonExtensionObject extensionData)
    {
        return !extensionData.ExtensionData.ContainsKey("x-jsonrpc") ? null : extensionData.ExtensionData["x-jsonrpc"] as JsonRpcExtension;
    }
}

using System.Text.Json;

namespace MediatRpc.Tools.OpenApi;
internal static class JsonRpcSample
{
    public static string Create(string method)
    {
        var request = new
        {
            id = Guid.NewGuid(),
            jsonrpc = "2.0",
            method,
            @params = new
            {
            }
        };

        return JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
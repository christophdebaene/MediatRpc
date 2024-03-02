using MediatRpc.Responses;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MediatRpc.JsonRpc;
public static class JsonRpcResults
{
    public static IResult Response(string id, object? response)
    {
        return response switch
        {
            FileResponse fileResponse => Results.File(fileResponse.Data, fileResponse.ContentType, fileResponse.Filename),
            JsonResponse jsonResponse => Response(id, jsonResponse),
            _ => Results.Json(new JsonRpcResponse
            {
                Id = id,
                Result = response
            })
        };
    }
    public static IResult Response(string id, JsonRpcError error) => Response(new JsonRpcErrorResponse
    {
        Id = id,
        Error = error
    });
    public static IResult Response(JsonRpcErrorResponse response) => Results.Json(response);

    internal static IResult Response(string id, JsonResponse response)
    {
        using (var stream = new MemoryStream())
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("id", id);
            writer.WriteString("jsonrpc", "2.0");
            writer.WritePropertyName("result");

            if (string.IsNullOrEmpty(response.Content))
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteRawValue(response.Content);
            }

            writer.WriteEndObject();
            writer.Flush();

            var json = Encoding.UTF8.GetString(stream.ToArray());
            return Results.Text(json, "application/json");
        }
    }
}

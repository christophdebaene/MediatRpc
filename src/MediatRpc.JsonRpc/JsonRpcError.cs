using System;
using System.Text.Json.Serialization;

namespace MediatRpc.JsonRpc;
public class JsonRpcError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public object Data { get; set; }
    public JsonRpcError()
    {
    }
    public JsonRpcError(int code, string message, object data = null)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public static readonly JsonRpcError InvalidRequest = new(JsonRpcErrorCode.InvalidRequest, "Invalid request");
    public static JsonRpcError MethodNotFound(string methodName) => new(JsonRpcErrorCode.MethodNotFound, $"Method '{methodName}' not found");
    public static readonly JsonRpcError InvalidParams = new(JsonRpcErrorCode.InvalidParams, "Invalid params");
    public static readonly JsonRpcError ParseError = new(JsonRpcErrorCode.ParseError, "Parse error");
    public static JsonRpcError InternalError(Exception exc) => new(JsonRpcErrorCode.InternalError, exc.Message, exc.Data);
}

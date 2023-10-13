using Microsoft.AspNetCore.Mvc;

namespace MediatRpc.JsonRpc;
public class JsonRpcResult : JsonResult
{
    public JsonRpcResult(JsonRpcResponse response) : base(response)
    {
        ContentType = "application/json";
    }
    public static JsonRpcResult Ok(string id, object result)
    {
        return new JsonRpcResult(new JsonRpcResponse
        {
            Id = id,
            Result = result
        });
    }
    public static JsonRpcResult Error(string id, JsonRpcError error)
    {
        return new JsonRpcResult(new JsonRpcErrorResponse
        {
            Id = id,
            Error = error
        });
    }
}

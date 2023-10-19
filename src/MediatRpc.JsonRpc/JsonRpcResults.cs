using Microsoft.AspNetCore.Http;

namespace MediatRpc.JsonRpc;
public static class JsonRpcResults
{
    public static IResult Ok(string id, object result) => Results.Json(new JsonRpcResponse
    {
        Id = id,
        Result = result
    });

    public static IResult Error(string id, JsonRpcError error) => Results.Json(new JsonRpcErrorResponse
    {
        Id = id,
        Error = error
    });
    public static IResult Response(string id, object response) => response is FileResponse fileResponse
               ? Results.File(fileResponse.Data, fileResponse.ContentType, fileResponse.Filename)
               : Ok(id, response);
    public static IResult InvalidRequest(string id) => Error(id, JsonRpcError.InvalidRequest);
    public static IResult MethodNotFound(string id, string methodName) => Error(id, JsonRpcError.MethodNotFound(methodName));    
    public static IResult InvalidParams(string id, string methodName) => Error(id, JsonRpcError.MethodNotFound(methodName));    
}

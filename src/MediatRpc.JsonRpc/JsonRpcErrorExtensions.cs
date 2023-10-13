using System.Net;

namespace MediatRpc.JsonRpc;
public static class JsonRpcErrorExtensions
{
    public static HttpStatusCode GetHttpStatusCode(this int errorCode)
    {
        return errorCode switch
        {
            JsonRpcErrorCode.InternalError => HttpStatusCode.InternalServerError,
            JsonRpcErrorCode.InvalidParams => HttpStatusCode.BadRequest,
            JsonRpcErrorCode.InvalidRequest => HttpStatusCode.BadRequest,
            JsonRpcErrorCode.MethodNotFound => HttpStatusCode.NotFound,
            JsonRpcErrorCode.ParseError => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}

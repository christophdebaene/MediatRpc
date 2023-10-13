using MediatRpc.JsonRpc;

namespace Sample.Api;
public class ExceptionStrategy : IExceptionStrategy
{
    public JsonRpcError Map(Exception exc, JsonRpcRequest request)
    {
        return new JsonRpcError
        {
            Code = JsonRpcErrorCode.InternalError,
            Message = exc.Message,
            Data = exc.Data
        };
    }
}

using System;

namespace MediatRpc.JsonRpc;
public interface IExceptionStrategy
{
    JsonRpcError Map(Exception exc, JsonRpcRequest request);
}

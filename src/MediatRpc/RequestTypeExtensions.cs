using System;

namespace MediatRpc;
public static class RequestTypeExtensions
{
    public static bool IsQuery(this Type type) => RequestTypeAttribute.Get(type) == RequestType.Query;
    public static bool IsCommand(this Type type) => RequestTypeAttribute.Get(type) == RequestType.Command;
}

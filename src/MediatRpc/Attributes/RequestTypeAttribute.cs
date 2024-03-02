using System;
using System.Reflection;

namespace MediatRpc;

[AttributeUsage(AttributeTargets.Class)]
public class RequestTypeAttribute(RequestType type) : Attribute
{
    public RequestType RequestType { get; } = type;
    public static RequestType Get(Type type)
    {
        var attr = type.GetCustomAttribute<RequestTypeAttribute>(false);
        return attr == null ? RequestType.Unknown : attr.RequestType;
    }
}

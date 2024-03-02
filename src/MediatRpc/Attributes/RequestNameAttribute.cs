using System;
using System.Linq;

namespace MediatRpc;

[AttributeUsage(AttributeTargets.Class)]
public class RequestNameAttribute : Attribute
{
    public RequestName Name { get; }

    public RequestNameAttribute(string name) : this(null, null, name)
    {
    }
    public RequestNameAttribute(string serviceName, string name) : this(null, serviceName, name)
    {
    }
    public RequestNameAttribute(string? @namespace, string? serviceName, string name)
    {
        Name = new RequestName(@namespace, serviceName, name);
    }
    public static RequestName? Get(Type type) => type.GetCustomAttributes(false)
            .OfType<RequestNameAttribute>()
            .FirstOrDefault()?.Name;
}
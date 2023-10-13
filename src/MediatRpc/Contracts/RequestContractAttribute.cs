using System;
using System.Linq;

namespace MediatRpc.Contracts;

[AttributeUsage(AttributeTargets.Class)]
public class RequestContractAttribute : Attribute
{
    public RequestContract Contract { get; }
    public RequestContractAttribute(string @namespace, string serviceName, string operationName)
    {
        Contract = new RequestContract(@namespace, serviceName, operationName);
    }
    public static RequestContract Get(Type type)
    {
        return type.GetCustomAttributes(false)
            .OfType<RequestContractAttribute>()
            .FirstOrDefault()?.Contract;
    }
}
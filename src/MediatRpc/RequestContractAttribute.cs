using System;
using System.Linq;

namespace MediatRpc;

[AttributeUsage(AttributeTargets.Class)]
public class RequestContractAttribute : Attribute
{
    public RequestContract Contract { get; }

    public RequestContractAttribute(string operationName) : this(null, null, operationName)
    {
        Contract = new RequestContract(null, null, operationName);
    }
    public RequestContractAttribute(string serviceName, string operationName) : this(null, serviceName, operationName)
    {
    }
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
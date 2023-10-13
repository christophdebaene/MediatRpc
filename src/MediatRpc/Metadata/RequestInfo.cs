using MediatR;
using MediatRpc.Contracts;
using System;
using System.Linq;

namespace MediatRpc.Metadata;
public readonly struct RequestInfo
{
    public RequestType Type { get; }
    public string Description { get; }
    public Type Request { get; }
    public Type Response { get; }
    public Type InterfaceHandler { get; }
    public Type ConcreteHandler { get; }
    public RequestContract Contract { get; }
    public RequestInfo(Type request, Type concreteHandler, RequestContract contract)
    {
        Type = RequestTypeAttribute.Get(request);
        Description = request.GetXmlDocumentation();
        Request = request;
        Response = request.GetResponseType();
        InterfaceHandler = request.GetInterfaces().Any(x => x.Name == "IRequest`1") ? typeof(IRequestHandler<,>).MakeGenericType(request, request.GetResponseType()) : typeof(IRequestHandler<>).MakeGenericType(request);
        ConcreteHandler = concreteHandler;
        Contract = contract;
    }
    public override string ToString() => Contract?.ToString();
}

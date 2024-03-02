using System;

namespace MediatRpc.Metadata;
public record EndpointInfo(RequestName Name, Type RequestType, Type? ResponseType)
{
    public override string ToString() => Name.ToString();
}
using MediatRpc.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MediatRpc.DependencyInjection;
public class MediatRpcConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    public bool VerifyRequestType { get; set; } = true;
    public Func<Type, RequestName> RequestName { get; set; } = x => new RequestName(null, null, x.Name);
    public IEndpointProvider EndpointProvider { get; set; } = new MediatREndpointProvider();
    public MediatRpcConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }
}
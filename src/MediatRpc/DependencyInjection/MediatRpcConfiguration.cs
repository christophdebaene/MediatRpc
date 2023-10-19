using System;
using System.Collections.Generic;
using System.Reflection;

namespace MediatRpc.DependencyInjection;
public class MediatRpcConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = new();
    public bool VerifyRequestType { get; set; } = false;
    public Func<Type, RequestContract> ContractResolver { get; set; } = x => new RequestContract(null, null, x.Name);
    public MediatRpcConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }
}
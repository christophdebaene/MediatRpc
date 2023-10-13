using MediatRpc.Contracts;
using System.Collections.Generic;
using System.Reflection;

namespace MediatRpc.Metadata;
public class TypeCatalogOptions
{
    public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
    public IRequestContractProvider ContractProvider { get; set; }
}

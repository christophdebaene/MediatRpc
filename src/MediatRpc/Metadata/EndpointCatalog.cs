using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatRpc.Metadata;
public class EndpointCatalog
{
    private readonly Dictionary<Type, EndpointInfo> _lookupByType;
    private readonly Dictionary<string, EndpointInfo> _lookupByContract;
    public IEnumerable<EndpointInfo> Endpoints { get; }
    public EndpointInfo this[Type request] => _lookupByType[request];
    public EndpointInfo this[string name] => _lookupByContract[name];
    public EndpointCatalog(IEnumerable<EndpointInfo> endpoints)
    {
        Endpoints = endpoints;
        _lookupByType = Endpoints.ToDictionary(x => x.RequestType);
        _lookupByContract = Endpoints.ToDictionary(x => x.Name.ToString());
    }
    public bool Exist(string name) => _lookupByContract.ContainsKey(name);
}

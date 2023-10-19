using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatRpc.Metadata;
public class TypeCatalog
{
    private Dictionary<Type, RequestInfo> _lookupByType => Requests.ToDictionary(x => x.Request);
    private Dictionary<string, RequestInfo> _lookupByContract => Requests.ToDictionary(x => x.Contract.ToString());
    public IEnumerable<RequestInfo> Requests { get; }
    public TypeCatalog(IEnumerable<RequestInfo> requests)
    {
        Requests = requests;
    }
    public RequestInfo this[Type request] => _lookupByType[request];
    public RequestInfo this[string name] => _lookupByContract[name];
    public bool Exist(string name) => _lookupByContract.ContainsKey(name);
}

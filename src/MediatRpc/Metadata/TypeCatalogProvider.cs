using MediatRpc.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace MediatRpc.Metadata;
public class TypeCatalogProvider
{
    private readonly TypeCatalogOptions _options;
    public TypeCatalogProvider(TypeCatalogOptions options)
    {
        _options = options;
    }
    public TypeCatalog Get()
    {
        var requests = new List<RequestInfo>();

        foreach (var requestType in _options.Assemblies.SelectMany(x => x.ScanRequestTypes()))
        {
            var handlers = _options.Assemblies.SelectMany(x => x.ScanRequestHandlerTypes(requestType));

            if (handlers.Count() > 1)
                throw new System.Exception($"Multiple handlers for {requestType.FullName}");

            _options.ContractProvider.TryGetRequestContract(requestType, out RequestContract contract);

            requests.Add(new RequestInfo(
                requestType,
                handlers.FirstOrDefault(),
                contract));
        }

        foreach (var request in requests.Where(x => x.Type == RequestType.Unknown))
            throw new System.Exception($"Request {request.ToString()} is unknown");

        return new TypeCatalog(requests);
    }
}

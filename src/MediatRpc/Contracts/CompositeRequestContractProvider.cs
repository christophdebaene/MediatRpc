using System;
using System.Collections.Generic;

namespace MediatRpc.Contracts;
public class CompositeRequestContractProvider : IRequestContractProvider
{
    private readonly IEnumerable<IRequestContractProvider> _providers;
    public CompositeRequestContractProvider(IEnumerable<IRequestContractProvider> providers)
    {
        _providers = providers;
    }
    public bool TryGetRequestContract(Type type, out RequestContract contract)
    {
        contract = null;

        foreach (var provider in _providers)
        {
            if (provider.TryGetRequestContract(type, out contract))
                return true;
        }

        return false;
    }
}

using System;

namespace MediatRpc.Contracts;
public class AttributeContractProvider : IRequestContractProvider
{
    public bool TryGetRequestContract(Type type, out RequestContract contract)
    {
        contract = RequestContractAttribute.Get(type);
        return contract != null;
    }
}

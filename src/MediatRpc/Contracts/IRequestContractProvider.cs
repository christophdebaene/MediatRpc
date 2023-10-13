using System;

namespace MediatRpc.Contracts;
public interface IRequestContractProvider
{
    bool TryGetRequestContract(Type type, out RequestContract contract);
}

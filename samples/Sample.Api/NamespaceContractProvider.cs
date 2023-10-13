using MediatRpc.Contracts;

namespace Sample.Api;
public class NamespaceContractProvider : IRequestContractProvider
{
    public bool TryGetRequestContract(Type type, out RequestContract contract)
    {
        var feature = type.Namespace!.Split(".").Last();
        contract = new RequestContract("MyApp", feature, type.Name);
        return true;
    }
}

using System.Linq;

namespace MediatRpc;
public record RequestContract(string Namespace, string ServiceName, string OperationName)
{
    public override string ToString()
    {
        string[] parts = [Namespace, ServiceName, OperationName];
        return string.Join(".", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}

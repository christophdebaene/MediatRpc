namespace MediatRpc.Contracts;
public record RequestContract(string Namespace, string ServiceName, string OperationName)
{
    public override string ToString() => $"{Namespace}.{ServiceName}.{OperationName}";
}

namespace MediatRpc.Tools.ClientApi.Model;

public class ClientService
{
    public string Name { get; set; }
    public List<ClientOperation> Operations { get; set; } = new List<ClientOperation>();
}
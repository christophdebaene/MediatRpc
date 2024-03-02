using NJsonSchema.CodeGeneration;

namespace MediatRpc.Tools.ClientApi.Model;

public class ClientModel
{
    public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public string Namespace { get; set; }
    public string Name { get; set; }
    public List<CodeArtifact> Definitions { get; set; } = [];
    public List<ClientService> Services { get; set; } = [];
    public ClientService this[string name]
    {
        get
        {
            var service = Services.SingleOrDefault(x => x.Name == name);
            if (service is null)
            {
                service = new ClientService
                {
                    Name = name
                };

                Services.Add(service);
            }

            return service;
        }
    }
}
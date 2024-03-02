using NJsonSchema.CodeGeneration.CSharp;

namespace MediatRpc.Tools.ClientApi;
public class ClientApiConfiguration
{
    public string Name { get; set; } = "MyApp";
    public CSharpGeneratorSettings CSharpGeneratorSettings { get; set; } = new CSharpGeneratorSettings
    {
        Namespace = "AppClient",
        ClassStyle = CSharpClassStyle.Poco,
    };
    public string RoutePrefix { get; set; } = "clientapi";
}




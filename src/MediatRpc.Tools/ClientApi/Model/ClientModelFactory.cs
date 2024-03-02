using MediatRpc.Tools.ClientApi.Extensions;
using MediatRpc.Tools.OpenApi;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using static MediatRpc.Tools.ClientApi.Extensions.OpenApiExtensions;

namespace MediatRpc.Tools.ClientApi.Model;

public static class ClientModelFactory
{
    public static ClientModel Create(OpenApiDocument document, ClientApiConfiguration configuration, ClientType clientType)
    {
        var resolver = CreateResolverWithExceptionSchema(configuration.CSharpGeneratorSettings, document);
        var csharpGenerator = new CSharpGenerator(document, configuration.CSharpGeneratorSettings, resolver);
        var definitions = csharpGenerator.GenerateTypes().ToList();

        var clientModel = new ClientModel
        {
            Name = configuration.Name,
            Namespace = configuration.CSharpGeneratorSettings.Namespace,
            Definitions = definitions
        };

        foreach (var pathItem in document.Paths)
        {
            var operation = pathItem.Value.First().Value;
            var jsonRpcExtension = operation.GetJsonRpcExtension();

            var contract = RequestName.Parse(operation.OperationId);
            var clientService = clientModel[contract.ServiceName];
            //var isFileRequest = operation?.RequestBody?.ActualName.Equals("formData", StringComparison.OrdinalIgnoreCase) ?? false;

            var schemaDescriptor = document.GetResponseType(operation);

            var clientOperation = new ClientOperation
            {
                Request = contract.Name,
                JsonRpcMethod = operation.OperationId,
                ResponseType = GetClientType(schemaDescriptor, ClientType.CSharp),
                IsFileRequest = jsonRpcExtension.isFileRequest
            };

            clientService.Operations.Add(clientOperation);
        }

        return clientModel;
    }

    static CSharpTypeResolver CreateResolverWithExceptionSchema(CSharpGeneratorSettings settings, OpenApiDocument document)
    {
        var exceptionSchema = document.Definitions.ContainsKey("Exception") ?
            document.Definitions["Exception"] : null;

        var resolver = new CSharpTypeResolver(settings, exceptionSchema);
        resolver.RegisterSchemaDefinitions(document.Definitions
            .Where(p => p.Value != exceptionSchema)
            .ToDictionary(p => p.Key, p => p.Value));

        return resolver;
    }
    static string GetClientType(SchemaDescriptor descriptor, ClientType type)
    {
        var name = descriptor.Name;

        if (string.IsNullOrEmpty(name) && descriptor.Schema is not null)
        {
            name = descriptor.Schema.Type.GetCSharpType();
        }

        if (descriptor.Schema is null)
        {
            return "Unit";
        }

        return descriptor.IsArray ? $"List<{name}>" : name;
    }
}

public static class JsonObjectTypeExtensions
{
    public static string GetCSharpType(this NJsonSchema.JsonObjectType type)
    {
        return type switch
        {
            NJsonSchema.JsonObjectType.String => "string",
            NJsonSchema.JsonObjectType.Number => "int",
            NJsonSchema.JsonObjectType.Boolean => "bool",
            NJsonSchema.JsonObjectType.Object => "object",
            NJsonSchema.JsonObjectType.Integer => "int",
            NJsonSchema.JsonObjectType.Null => "Unit",
            _ => "object",
        };
    }
}



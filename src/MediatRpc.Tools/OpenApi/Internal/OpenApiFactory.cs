using MediatRpc.Metadata;
using NSwag;
using System.Data;

namespace MediatRpc.Tools.OpenApi;
internal class OpenApiFactory
{
    public static OpenApiDocument Create(EndpointCatalog catalog, OpenApiConfiguration configuration)
    {
        var document = new OpenApiDocument
        {
            SchemaType = NJsonSchema.SchemaType.OpenApi3
        };

        var jsonSchemaFactory = new JsonSchemaFactory(document, configuration.Settings);
        jsonSchemaFactory.ScanCatalog(catalog);

        document.Info = new OpenApiInfo
        {
            Title = "API Specification",
            Description = "API Description"
        };

        document.Produces.Add("application/json");
        document.Consumes.Add("application/json");

        foreach (var request in catalog.Endpoints.OrderBy(x => x.Name.ServiceName))
        {
            document.Paths.Add($"/{request.Name.ServiceName}/{request.Name.Name}", new OpenApiPathItem
            {
                {
                    OpenApiOperationMethod.Post,
                    CreateOperation(configuration, jsonSchemaFactory, request)
                }
            });
        }

        return document;
    }
    internal static OpenApiOperation CreateOperation(OpenApiConfiguration configuration, JsonSchemaFactory jsonSchemaFactory, EndpointInfo request)
    {
        return new OpenApiOperation
        {
            Summary = $"{request.Name.Name}",
            OperationId = request.Name.ToString(),
            Parameters =
            {
                new OpenApiParameter
                {
                    Kind = OpenApiParameterKind.Body,
                    AllowEmptyValue = false,
                    IsRequired = true,
                    Schema = jsonSchemaFactory.CreateJsonRpcRequest(request)
                }
            },
            Responses =
            {
                {
                    "200",
                    new OpenApiResponse
                    {
                        Description = "Ok",
                        Schema = jsonSchemaFactory.CreateJsonRpcResponse(request)
                    }
                }
            },
            Tags = [request.Name.ServiceName],
            ExtensionData = new Dictionary<string, object?>
            {
                /*
                {
                    "x-custom-examples", new List<object>
                    {
                        new
                        {
                            lang = "Shell-cURL",
                            label = "JsonRpc Request",
                            source = JsonRpcSample.Create(request.Contract.ToString())
                        }
                    }
                },
                */
                {
                    "x-jsonrpc", new JsonRpcExtension
                    {
                        isFileRequest = typeof(IFileRequest).IsAssignableFrom(request.RequestType)
                    }
                }
            }
        };
    }
}

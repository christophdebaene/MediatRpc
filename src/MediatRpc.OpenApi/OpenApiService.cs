using MediatRpc.Metadata;
using NJsonSchema.Generation;
using NSwag;
using System.Text.Json;

namespace MediatRpc.OpenApi;
public class OpenApiService
{
    public static OpenApiDocument Create(RequestCatalog catalog, OpenApiConfiguration configuration)
    {
        var document = new OpenApiDocument
        {
            Info = configuration.Info
        };

        document.Produces.Add("application/json");
        document.Consumes.Add("application/json");

        var schemaGenerator = new JsonSchemaGenerator(configuration.GeneratorSettings);
        //var schemaResolver = new OpenApiSchemaResolver(document, configuration.GeneratorSettings);

        /*
        var types = catalog.Requests.Select(x => x.Request)
            .Union(catalog.Requests.Select(x => x.Response))
            .Distinct();
        
        foreach (var type in types)
            schemaGenerator.Generate(type, schemaResolver);
        */
                     
        foreach (var request in catalog.Requests.OrderBy(x => x.Contract.ServiceName))
        {
            
            document.Paths.Add($"/{request.Contract.ServiceName}/{request.Contract.OperationName}", new OpenApiPathItem
            {
                {
                    request.Type == RequestType.Query ? OpenApiOperationMethod.Get : OpenApiOperationMethod.Post,
                    new OpenApiOperation
                    {
                        Summary = $"{request.Contract.OperationName}",
                        Description = configuration.Description(request),
                        Parameters =
                        {
                            new OpenApiParameter
                            {
                                Kind = OpenApiParameterKind.Body,
                                AllowEmptyValue = false,
                                IsRequired = true,
                                Schema = schemaGenerator.Generate(request.Request)
                            }
                        },
                        Responses =
                        {
                            {
                                "200",
                                new OpenApiResponse
                                {
                                    Schema = schemaGenerator.Generate(request.Response)
                                }
                            }
                        },
                        /*
                        Tags = new List<string> { request.Contract.ServiceName },
                        ExtensionData = new Dictionary<string, object>
                        {
                                {
                                "x-code-samples", new List<object>
                                    {
                                        new {
                                            lang = "JSON-RPC",
                                            source = CreateSampleRpcRequest(request.Contract.ToString())
                                        }
                                    }
                            },
                            {
                                "x-jsonrpc-contract", new
                                {
                                    operation = request.Contract.OperationName,
                                    service = request.Contract.ServiceName,
                                    @namespace = request.Contract.Namespace,
                                    fullName = request.Contract.ToString(),
                                    isFileRequest = typeof(IFileRequest).IsAssignableFrom(request.Request),
                                    isFileResponse = typeof(FileResponse).IsAssignableFrom(request.Response),
                                    isParams = typeof(IHaveParams).IsAssignableFrom(request.Request),
                                }
                            }
                        }
                        */
                    }
                }
            });
        }

        return document;
    }
    internal static string CreateSampleRpcRequest(string method)
    {
        var request = new
        {
            id = Guid.NewGuid(),
            jsonrpc = "2.0",
            method = method,
            @params = new
            {
            }
        };

        return JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
    }
}

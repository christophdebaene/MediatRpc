using MediatRpc.Metadata;
using MediatRpc.OpenApi.Utils;
using NJsonSchema.Generation;
using NSwag;
using System.Text.Json;

namespace MediatRpc.OpenApi;
public class OpenApiConfiguration
{
    public OpenApiInfo Info { get; set; }
    public Func<RequestInfo, string> Description { get; set; } = (x) => x.Description;
    public JsonSchemaGeneratorSettings GeneratorSettings { get; set; } = new SystemTextJsonSchemaGeneratorSettings
    {           
        FlattenInheritanceHierarchy = true,
        SerializerOptions = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Converters =
            {            
            },
        }
    };
    public OpenApiConfiguration()
    {
        Info = new OpenApiInfo
        {
            Title = "API Specification",
            Description = ResourceService.Get("JsonRpcDescription")
        };
    }
}
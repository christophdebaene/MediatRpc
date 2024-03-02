using NJsonSchema.Generation;
using NSwag;
using System.Text.Json;

namespace MediatRpc.JsonRpc.OpenApi;

public class OpenApiConfiguration
{
    public string RoutePrefix { get; set; } = "/openapi";
    public static SystemTextJsonSchemaGeneratorSettings DefaultSettings { get; } = new SystemTextJsonSchemaGeneratorSettings
    {
        FlattenInheritanceHierarchy = true,
        SchemaType = NJsonSchema.SchemaType.OpenApi3,
    };
    public SystemTextJsonSchemaGeneratorSettings Settings { get; set; } = DefaultSettings;
    public Action<OpenApiDocument>? PostProcess { get; set; }
    public Action<JsonSerializerOptions> JsonSerializerOptionsPostProcess { get; set; }
}
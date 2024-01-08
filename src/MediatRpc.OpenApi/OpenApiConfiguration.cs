using MediatRpc.Metadata;
using MediatRpc.OpenApi.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using NSwag;

namespace MediatRpc.OpenApi;
public class OpenApiConfiguration
{
    public OpenApiInfo Info { get; set; }
    public Func<RequestInfo, string> Description { get; set; } = (x) => x.Description;
    public JsonSchemaGeneratorSettings GeneratorSettings { get; set; } = new JsonSchemaGeneratorSettings
    {           
        FlattenInheritanceHierarchy = true,        
        SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy()
            },
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
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
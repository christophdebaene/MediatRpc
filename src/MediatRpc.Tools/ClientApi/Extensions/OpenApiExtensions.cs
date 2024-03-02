using NJsonSchema;
using NSwag;

namespace MediatRpc.Tools.ClientApi.Extensions;

internal static class OpenApiExtensions
{
    public static SchemaDescriptor? GetResponseType(this OpenApiDocument document, OpenApiOperation operation)
    {
        var schema = operation.Responses["200"].Schema;
        if (schema is null)
        {
            return default;
        }

        if (schema.HasReference)
        {
            schema = schema.Reference;
        }

        bool isArray = false;

        if (schema.IsArray)
        {
            isArray = true;
            schema = schema.Item.Reference;
        }

        var responseType = document.Definitions.FirstOrDefault(x => x.Value == schema);
        if (string.IsNullOrEmpty(responseType.Key))
        {
            return new(responseType.Key, isArray, schema);
        }
        else
        {
            return new(responseType.Key, isArray, responseType.Value);
        }
    }
    public record SchemaDescriptor(string Name, bool IsArray, NJsonSchema.JsonSchema Schema);
}

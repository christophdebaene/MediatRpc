using MediatRpc.Metadata;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag;

namespace MediatRpc.Tools.OpenApi;

internal class JsonSchemaFactory(OpenApiDocument document, JsonSchemaGeneratorSettings settings)
{
    private readonly JsonSchemaGenerator _generator = new(settings);
    private readonly OpenApiSchemaResolver _resolver = new(document, settings);

    public void ScanCatalog(EndpointCatalog catalog)
    {
        foreach (var requestInfo in catalog.Endpoints)
        {
            GetOrCreate(requestInfo.RequestType);
            if (requestInfo.ResponseType is not null)
                GetOrCreate(requestInfo.ResponseType);
        }
    }
    public JsonSchema GetOrCreate(Type type)
    {
        return _generator.Generate(type, _resolver);
    }
    public JsonSchema CreateJsonRpcResponse(EndpointInfo requestInfo)
    {
        if (requestInfo.ResponseType is null)
        {
            return new JsonSchemaProperty
            {
                Type = JsonObjectType.Null,
                IsRequired = true
            };
        }
        else
        {
            return GetOrCreate(requestInfo.ResponseType);
        }

        /*
        var jsonSchema = new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "id",
                    new JsonSchemaProperty
                    {
                        Default = "b9df3ee6-c49d-4799-b9ce-d9a9b28f67a9",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "jsonrpc",
                    new JsonSchemaProperty
                    {
                        Default = "2.0",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "result",
                    requestInfo.Response is null ?
                        new JsonSchemaProperty
                        {
                            Type = JsonObjectType.Null,
                            IsRequired = true
                        } :
                        new JsonSchemaProperty
                        {
                            Reference = GetOrCreate(requestInfo.Response)
                        }
                }
            }
        };

        return jsonSchema;
        */
    }
    public JsonSchema CreateJsonRpcRequest(EndpointInfo requestInfo)
    {
        var jsonSchema = new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "id",
                    new JsonSchemaProperty
                    {
                        Default = "b9df3ee6-c49d-4799-b9ce-d9a9b28f67a9",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "jsonrpc",
                    new JsonSchemaProperty
                    {
                        Default = "2.0",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "method",
                    new JsonSchemaProperty
                    {
                        Default = requestInfo.Name.ToString(),
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "params",
                    new JsonSchemaProperty
                    {
                        Reference = GetOrCreate(requestInfo.RequestType)
                    }
                },
            }
        };

        return jsonSchema;
    }
}
using MediatRpc.Metadata;
using MediatRpc.Tools.ClientApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSwag;

namespace MediatRpc.Tools.OpenApi;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, Action<OpenApiConfiguration>? configuration = null)
    {
        var config = new OpenApiConfiguration();
        configuration?.Invoke(config);
        return services.AddOpenApi(config);
    }
    public static IServiceCollection AddOpenApi(this IServiceCollection services, OpenApiConfiguration configuration = null)
    {
        var jsonOptions = services.AddScoped((serviceProvider) =>
        {
            var serializerOptions = serviceProvider.GetService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>()!.Value.SerializerOptions;
            serializerOptions ??= new System.Text.Json.JsonSerializerOptions
            {
            };

            if (configuration.JsonSerializerOptionsPostProcess is not null)
                configuration.JsonSerializerOptionsPostProcess(serializerOptions);

            configuration.Settings.SerializerOptions = serializerOptions;

            var catalog = serviceProvider.GetRequiredService<EndpointCatalog>();

            var openApiDocument = OpenApiFactory.Create(catalog, configuration);

            if (configuration.PostProcess is not null)
                configuration.PostProcess(openApiDocument);

            return openApiDocument;
        });

        return services;
    }
    public static IEndpointRouteBuilder UseOpenApi(this IEndpointRouteBuilder endpoints, string routePrefix = "/openapi")
    {
        endpoints.MapGet(routePrefix, (OpenApiDocument openApiDocument, string format = "json", bool highlight = false) =>
        {
            var openApi = format == "json" ? openApiDocument.ToJson() : openApiDocument.ToYaml();
            return highlight ? new HighlightCodeResult("OpenApi Specification", openApi, format) : format == "json" ? Results.Text(openApi, "application/json") : Results.Text(openApi, "text/plain");
        });

        return endpoints;
    }
}

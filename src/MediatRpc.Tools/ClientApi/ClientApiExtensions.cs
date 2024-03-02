using MediatRpc.Tools.ClientApi.Model;
using MediatRpc.Tools.Templates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using NSwag;

namespace MediatRpc.Tools.ClientApi;

public static class ClientApiExtensions
{
    public static IEndpointRouteBuilder UseClientApi(this IEndpointRouteBuilder endpoints, Action<ClientApiConfiguration>? configuration = null)
    {
        var config = new ClientApiConfiguration();
        configuration?.Invoke(config);
        return endpoints.UseClientApi(config);
    }
    public static IEndpointRouteBuilder UseClientApi(this IEndpointRouteBuilder endpoints, ClientApiConfiguration configuration)
    {
        endpoints.MapGet(configuration.RoutePrefix, (OpenApiDocument document) =>
        {
            var language = ClientType.CSharp;

            var clientModel = ClientModelFactory.Create(document, configuration, language);
            var code = TemplateRenderer.Render("CSharp.handlebars", clientModel);

            return new HighlightCodeResult("CSharp Client", code, "csharp");
        });

        return endpoints;
    }
}

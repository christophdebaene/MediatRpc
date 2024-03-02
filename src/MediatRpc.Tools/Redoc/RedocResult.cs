using MediatRpc.Tools.Templates;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MediatRpc.Tools.Redoc;

public class RedocResult(RedocConfiguration configuration) : IResult
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var redoc = GetRedocHtml();

        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(redoc);
        return httpContext.Response.WriteAsync(redoc);
    }
    string GetRedocHtml()
    {
        var model = new
        {
            Title = configuration.Title,
            SpecUrl = configuration.SpecUrl,
            Options = JsonSerializer.Serialize(configuration.Options, _serializerOptions)
        };

        return TemplateRenderer.Render("Redoc.html", model);
    }
}
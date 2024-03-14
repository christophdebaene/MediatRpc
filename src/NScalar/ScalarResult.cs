using Microsoft.AspNetCore.Http;
using NScalar.Templates;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace NScalar;

public class ScalarResult(ScalarConfiguration configuration) : IResult
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var redoc = GetScalarHtml();

        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(redoc);
        return httpContext.Response.WriteAsync(redoc);
    }
    string GetScalarHtml()
    {
        var model = new Dictionary<string, string>
        {
            { "Title", configuration.Title },
            { "SpecUrl", configuration.SpecUrl },
            { "ProxyUrl", configuration.ProxyUrl },
            { "Options", JsonSerializer.Serialize(configuration.Options, _serializerOptions) }
        };

        return TemplateRenderer.Render(model);
    }
}
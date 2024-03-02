using MediatRpc.Tools.Templates;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace MediatRpc.Tools.ClientApi;

public class HighlightCodeResult(string title, string code, string language) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        var encodedCode = HttpUtility.HtmlEncode(code);
        var highlightCode = TemplateRenderer.Render("HighlightCode.html", new { title, code = encodedCode, language });
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(highlightCode);
        return httpContext.Response.WriteAsync(highlightCode);
    }
}

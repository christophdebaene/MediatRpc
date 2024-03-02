using HandlebarsDotNet;
using MediatRpc.Tools.Redoc;

namespace MediatRpc.Tools.Templates;
internal static class TemplateRenderer
{
    static TemplateRenderer()
    {
        Handlebars.Configuration.TextEncoder = null;
    }
    public static string Render(string template, object arg)
    {
        var templateContent = GetEmbeddedResource(template);
        var templateHb = Handlebars.Compile(templateContent);
        return templateHb(arg);
    }
    internal static string GetEmbeddedResource(string name)
    {
        using (var stream = typeof(RedocResult).Assembly.GetManifestResourceStream("MediatRpc.Tools.Templates." + name))
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
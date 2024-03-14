namespace NScalar.Templates;

public static class TemplateRenderer
{
    public static string Render(Dictionary<string, string> args)
    {
        var template = GetTemplate();
        foreach (var arg in args)
        {
            template = template.Replace($"%{arg.Key}%", arg.Value);
        }

        return template;
    }
    internal static string GetTemplate()
    {
        using var stream = typeof(TemplateRenderer).Assembly.GetManifestResourceStream("NScalar.Templates.Scalar.html");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

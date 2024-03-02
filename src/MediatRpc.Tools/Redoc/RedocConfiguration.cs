namespace MediatRpc.Tools.Redoc;

public class RedocConfiguration
{
    public string RoutePrefix { get; set; } = "/redoc";
    public string Title { get; set; } = "Redoc";
    public string SpecUrl { get; set; } = "/openapi";
    public RedocOptions Options { get; set; } = new RedocOptions();
}

public class RedocOptions
{
    /// <summary>
    /// Hides the loading animation
    /// </summary>
    public bool HideLoading { get; set; } = false;
}

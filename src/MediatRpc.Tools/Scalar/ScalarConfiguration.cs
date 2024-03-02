namespace MediatRpc.Tools.Scalar;

public class ScalarConfiguration
{
    public string RoutePrefix { get; set; } = "/scalar";
    public string Title { get; set; } = "Scalar";
    public string SpecUrl { get; set; } = "/openapi";

    public ScalarOptions Options { get; set; } = new ScalarOptions();
}
public class ScalarOptions
{
}
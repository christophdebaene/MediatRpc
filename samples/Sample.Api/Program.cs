using MediatR;
using MediatRpc;
using MediatRpc.DependencyInjection;
using MediatRpc.JsonRpc;
using MediatRpc.Metadata;
using MediatRpc.OpenApi;
using Microsoft.EntityFrameworkCore;
using Sample.Domain;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblies = new List<Assembly> { Assembly.Load("Sample.Application") }.ToArray();

builder.Services.AddDbContext<TodoContext>((options =>
{
    options.UseInMemoryDatabase("Todo");
}));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddMediatRpc(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
    cfg.ContractResolver = type => new RequestContract("MyApp", type.Namespace!.Split(".").Last(), type.Name);
});

var app = builder.Build();

app.MapGet("/openapi", (RequestCatalog catalog) =>
{
    return ReDocResults.OpenApi(catalog);
    //var openApiDocument = OpenApiService.Create(catalog, new OpenApiConfiguration());
    //return openApiDocument.ToJson();
});

app.MapGet("/redoc", () =>
{
    return ReDocResults.Response();
});

app.MapPost("/jsonrpc", async (ISender sender, JsonRpcRequest jsonRpcRequest, MediatRequest request, CancellationToken cancellationToken) =>
{
    try
    {
        var response = await sender.Send(request.Message, cancellationToken);
        return JsonRpcResults.Response(jsonRpcRequest.Id, response);
    }
    catch (Exception exc)
    {
        return JsonRpcResults.Error(jsonRpcRequest.Id, JsonRpcError.InternalError(exc));
    }
});

app.Run();
using MediatR;
using MediatRpc;
using MediatRpc.DependencyInjection;
using MediatRpc.JsonRpc;
using MediatRpc.Metadata;
using Microsoft.AspNetCore.Mvc;
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

app.MapPost("/jsonrpc", async (ISender sender, TypeCatalog catalog, [FromBody] JsonRpcRequest jsonRpcRequest, CancellationToken cancellationToken) =>
{
    if (!catalog.Exist(jsonRpcRequest.Method))
        JsonRpcResults.MethodNotFound(jsonRpcRequest.Id, jsonRpcRequest.Method);

    var requestInfo = catalog[jsonRpcRequest.Method];
    var request = jsonRpcRequest.CreateInstance(requestInfo.Request);

    try
    {
        var response = await sender.Send(request, cancellationToken);
        return JsonRpcResults.Response(jsonRpcRequest.Id, response);
    }
    catch (Exception ex)
    {
        return JsonRpcResults.Error(jsonRpcRequest.Id, new JsonRpcError
        {
            Code = JsonRpcErrorCode.InternalError,
            Message = ex.Message,
            Data = ex.Data
        });
    }
});

app.Run();
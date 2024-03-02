using MediatRpc;
using MediatRpc.DependencyInjection;
using MediatRpc.JsonRpc.OpenApi;
using Microsoft.EntityFrameworkCore;
using NScalar;
using Sample.Api.Endpoints;
using Sample.Application;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblies = new List<Assembly> { Assembly.Load("Sample.Application") }.ToArray();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseInMemoryDatabase("Sample");
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddMediatRpc(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
    cfg.RequestName = type => new RequestName("MyApp", type.Namespace!.Split(".").Last(), type.Name);
});
builder.Services.AddJsonRpcOpenApi((config) => { });


var app = builder.Build();
app.UseScalar();
app.UseJsonRpcOpenApi();
app.MapJsonRpc();
app.MapScalar();

app.Run();
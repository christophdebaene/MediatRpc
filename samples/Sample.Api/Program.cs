using MediatRpc;
using MediatRpc.DependencyInjection;
using MediatRpc.Tools.OpenApi;
using MediatRpc.Tools.Redoc;
using MediatRpc.Tools.Scalar;
using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();
app.UseOpenApi();
app.UseScalar();
app.UseRedoc();
app.MapJsonRpc();

app.Run();
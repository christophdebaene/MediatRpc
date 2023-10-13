using MediatRpc.JsonRpc;
using Microsoft.EntityFrameworkCore;
using Sample.Api;
using Sample.Domain;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblies = new List<Assembly> { Assembly.Load("Sample.Application") }.ToArray();

builder.Services.AddDbContext<TodoContext>((options =>
{
    options.UseInMemoryDatabase("Todo");
}));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddJsonRpc(new NamespaceContractProvider(), assemblies);
builder.Services.UseJsonRpc(typeof(ExceptionStrategy));

var app = builder.Build();
app.MapControllers();
app.Run();
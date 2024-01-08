using MediatRpc.Metadata;
using MediatRpc.OpenApi.Utils;
using Microsoft.AspNetCore.Http;

namespace MediatRpc.OpenApi;
public static class ReDocResults
{
    public static IResult OpenApi(RequestCatalog catalog)
    {
        var openApiDocument = OpenApiService.Create(catalog, new OpenApiConfiguration());
        return Results.Text(openApiDocument.ToJson(), "application/json");

        /*
        return openApiDocument.ToJson();

        var redoc = ResourceService.Get("ReDoc").Replace("/openapi", openApiPath);
        return Results.Content(openApiDocument.ToJson(), ");
        */
    }
    public static IResult Response(string openApiPath = "/openapi")
    {
        var redoc = ResourceService.Get("ReDoc").Replace("/openapi", openApiPath);
        return Results.Content(redoc, "text/html");
    }
}
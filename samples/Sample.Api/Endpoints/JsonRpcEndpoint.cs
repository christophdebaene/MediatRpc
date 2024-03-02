using MediatR;
using MediatRpc.JsonRpc;

namespace Sample.Api.Endpoints;
public static class JsonRpcEndpoint
{
    public static WebApplication MapJsonRpc(this WebApplication app)
    {
        app.MapPost("/jsonrpc", async (HttpContext context, ISender sender, JsonRpcEndpointResult endpointResult, CancellationToken cancellationToken) =>
        {
            if (!endpointResult.IsValid)
            {
                return JsonRpcResults.Response(endpointResult.Error);
            }

            var endpoint = endpointResult.Endpoint;

            try
            {
                var message = endpoint.CreateMessage(context);
                var response = await sender.Send(message, cancellationToken);
                return JsonRpcResults.Response(endpoint.Request.Id, response);
            }
            catch (Exception exc)
            {
                var errorResponse = new JsonRpcErrorResponse
                {
                    Id = endpoint.Request.Id,
                    Error = new JsonRpcError
                    {
                        Code = JsonRpcErrorCode.InvalidRequest,
                        Message = exc.Message,
                        Data = exc.Data
                    }
                };

                return JsonRpcResults.Response(errorResponse);
            };
        });

        return app;
    }
}

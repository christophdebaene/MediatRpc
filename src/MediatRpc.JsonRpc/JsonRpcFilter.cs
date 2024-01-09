using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;

public class JsonRpcFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var jsonRpc = GetJsonRpc(context.HttpContext.Request);

        return await next(context);
    }
    static string GetJsonRpc(HttpRequest request)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        if (request.HasFormContentType)
        {
            return request.Form["jsonRpc"];            
        }
        else
        {
            request.Body.Position = 0;
            return new StreamReader(request.Body).ReadToEnd();
        }
    }
}

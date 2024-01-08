using MediatRpc.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;
public class MediatRequest
{
    public RequestInfo Info { get; set; }
    public object Message { get; set; }
    public static async ValueTask<MediatRequest> BindAsync(HttpContext context)
    {
        var jsonRpcRequest = await JsonRpcRequest.BindAsync(context);
        var catalog = context.RequestServices.GetRequiredService<RequestCatalog>();

        var requestInfo = catalog[jsonRpcRequest.Method];
        var request = jsonRpcRequest.CreateInstance(requestInfo.Request);

        if (context.Request.HasFormContentType)
        {
            var files = context.Request.Form.Files;
            if (request is IFileRequest fileRequest)
            {
                fileRequest.Files = files.Select(x => new FileProxy(x)).Cast<IFile>().ToList().AsReadOnly();
            }
        }
        
        return new MediatRequest
        {
            Info = requestInfo,
            Message = request
        };
    }
}

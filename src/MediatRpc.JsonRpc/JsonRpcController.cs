using MediatR;
using MediatRpc.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;

[Route("jsonrpc")]
[ApiController]
[DisableRequestSizeLimit]
public class JsonRpcController : ControllerBase
{
    private readonly TypeCatalog _typeCatalog;
    private readonly IExceptionStrategy _exceptionStrategy;
    public JsonRpcController(TypeCatalog typeCatalog, IExceptionStrategy exceptionStrategy)
    {
        _typeCatalog = typeCatalog ?? throw new ArgumentNullException(nameof(typeCatalog));
        _exceptionStrategy = exceptionStrategy ?? throw new ArgumentNullException(nameof(exceptionStrategy));
    }

    [HttpOptions]
    public IActionResult Options()
    {
        HttpContext.Response.Headers.AppendCommaSeparatedValues(
            HeaderNames.Allow,
            HttpMethods.Get,
            HttpMethods.Head,
            HttpMethods.Options,
            HttpMethods.Post);
        return Ok();
    }

    [HttpGet]
    [Consumes("application/json")]
    public ContentResult Get()
        => Content("2.0");

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post([FromBody] JsonRpcRequest jsonRpcRequest, CancellationToken cancellationToken)
    {
        if (!_typeCatalog.Exist(jsonRpcRequest.Method))
            return JsonRpcResult.Error(jsonRpcRequest.Id, JsonRpcError.MethodNotFound(jsonRpcRequest.Method));

        var requestProcessor = HttpContext.RequestServices.GetService<ISender>();
        return await SendAsync(requestProcessor, jsonRpcRequest, cancellationToken);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Post([FromForm] string jsonRpc, [FromForm(Name ="files")] IFormFileCollection files, CancellationToken cancellationToken)
    {
        var jsonRpcRequest = JsonRpcRequest.Create(jsonRpc);

        if (!_typeCatalog.Exist(jsonRpcRequest.Method))
            return JsonRpcResult.Error(jsonRpcRequest.Id, JsonRpcError.MethodNotFound(jsonRpcRequest.Method));

        Action<object> requestAction = (request) =>
        {
            if (request is IFileRequest fileRequest)
                fileRequest.Files = files.Select(x => new FileProxy(x)).Cast<IFile>().ToList().AsReadOnly();
        };

        var requestProcessor = HttpContext.RequestServices.GetService<ISender>();

        return await SendAsync(requestProcessor, jsonRpcRequest, cancellationToken, requestAction);
    }
    async Task<IActionResult> SendAsync(ISender processor, JsonRpcRequest jsonRpcRequest, CancellationToken cancellationToken, Action<object> requestAction = null)
    {
        var requestInfo = _typeCatalog[jsonRpcRequest.Method];
        var request = jsonRpcRequest.CreateInstance(requestInfo.Request);

        requestAction?.Invoke(request);

        try
        {
            var response = await processor.Send(request, cancellationToken);
            return response is FileResponse fileResponse
                ? File(fileResponse.Data, fileResponse.ContentType, fileResponse.Filename)
                : JsonRpcResult.Ok(jsonRpcRequest.Id, response);
        }
        catch (Exception exc)
        {
            var error = _exceptionStrategy.Map(exc, jsonRpcRequest);
            return JsonRpcResult.Error(jsonRpcRequest.Id, error);
        }
    }
}

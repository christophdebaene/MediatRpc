using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;

namespace MediatRpc.JsonRpc;
public class JsonRpcEndpoint(RequestName name, Type requestType, JsonRpcRequest request)
{
    public RequestName Name { get; } = name;
    public Type RequestType { get; } = requestType;
    public JsonRpcRequest Request { get; } = request;
    public object? CreateMessage(HttpContext context)
    {
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

        var request = Request.Params.ValueKind == JsonValueKind.Undefined ?
            Activator.CreateInstance(RequestType) :
            JsonSerializer.Deserialize(Request.Params, RequestType, serializerOptions);

        if (request is IHaveId iHaveId)
            iHaveId.Id = Request.Id;

        if (request is IHaveParams iHaveParams)
            iHaveParams.Params = Request.Params;

        if (context.Request.HasFormContentType)
        {
            var files = context.Request.Form.Files;
            if (files is not null && request is IFileRequest fileRequest)
            {
                fileRequest.Files = files.Select(x => new FileProxy(x)).Cast<IFile>().ToList().AsReadOnly();
            }
        }

        return request;
    }
}

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc.Extensions;
internal static class HttpRequestExtensions
{
    public static async Task<IEnumerable<IFormFile>> BindFilesAsync(this HttpRequest request)
    {
        var files = new List<IFormFile>();

        if (request.HasFormContentType)
        {
            var form = await request.ReadFormAsync();

            foreach (var file in form.Files)
            {                
                if (file.Length == 0 && string.IsNullOrEmpty(file.FileName))
                {
                    continue;
                }

                files.Add(file);
            }
        }

        return files;
    }
    public static async Task<string> GetRawBodyStringAsync(this HttpRequest request)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        request.Body.Position = 0;

        var reader = new StreamReader(request.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return body;
    }
}
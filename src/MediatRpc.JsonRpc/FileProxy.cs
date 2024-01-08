using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;
internal class FileProxy(IFormFile file) : IFile
{    
    public string ContentType => file.ContentType;
    public string ContentDisposition => file.ContentDisposition;
    public long Length => file.Length;
    public string Name => file.Name;
    public string FileName => file.FileName;
    public void CopyTo(Stream target) => file.CopyTo(target);
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => file.CopyToAsync(target, cancellationToken);
    public Stream OpenReadStream() => file.OpenReadStream();
}

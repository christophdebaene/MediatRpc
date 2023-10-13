using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRpc.JsonRpc;
internal class FileProxy : IFile
{
    private readonly IFormFile _file;
    public string ContentType => _file.ContentType;
    public string ContentDisposition => _file.ContentDisposition;
    public long Length => _file.Length;
    public string Name => _file.Name;
    public string FileName => _file.FileName;
    public FileProxy(IFormFile file) => _file = file;
    public void CopyTo(Stream target) => _file.CopyTo(target);
    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => _file.CopyToAsync(target, cancellationToken);
    public Stream OpenReadStream() => _file.OpenReadStream();
}

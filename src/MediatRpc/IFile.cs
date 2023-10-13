using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRpc;
public interface IFile
{
    string ContentType { get; }
    string ContentDisposition { get; }
    long Length { get; }
    string Name { get; }
    string FileName { get; }
    Stream OpenReadStream();
    void CopyTo(Stream target);
    Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);    
}
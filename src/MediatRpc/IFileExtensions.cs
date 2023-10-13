using System.IO;
using System.Threading.Tasks;

namespace MediatRpc;
public static class IFileExtensions
{
    public static async Task<string> ReadAsTextAsync(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }
    }
    public static async Task<byte[]> ReadAsBytesAsync(this IFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}
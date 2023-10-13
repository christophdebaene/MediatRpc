using System.Collections.Generic;

namespace MediatRpc;
public interface IFileRequest
{
    IReadOnlyList<IFile> Files { get; set; }
}

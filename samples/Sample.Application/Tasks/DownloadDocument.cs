using MediatR;
using MediatRpc;
using Sample.Domain;

namespace Sample.Application.Tasks;

[Query]
public record DownloadDocument : IRequest<FileResponse>
{
    public string FileId { get; init; }
}
public class DownloadDocumentHandler : IRequestHandler<DownloadDocument, FileResponse>
{
    private readonly TodoContext _context;
    public DownloadDocumentHandler(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<FileResponse> Handle(DownloadDocument request, CancellationToken cancellationToken)
    {
        var document = await _context.Documents.FindAsync(request.FileId, cancellationToken);
        var response = new FileResponse
        {
            Filename = document.Filename,
            ContentType = document.ContentType,
            Data = document.Data
        };

        return response;
    }
}

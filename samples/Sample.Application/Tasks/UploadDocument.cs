using MediatR;
using MediatRpc;
using Sample.Domain;

namespace Sample.Application.Tasks;

[Command]
public record UploadDocument : IRequest, IFileRequest
{
    public Guid TodoId { get; init; }
    public IReadOnlyList<IFile> Files { get; set; }
}
public class UploadDocumentHandler : IRequestHandler<UploadDocument>
{
    private readonly TodoContext _context;
    public UploadDocumentHandler(TodoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task Handle(UploadDocument request, CancellationToken cancellationToken)
    {
        var documents = new List<Document>();

        foreach (var file in request.Files)
        {
            documents.Add(new Document
            {
                Id = Guid.NewGuid(),
                Filename = file.FileName,
                ContentType = file.ContentType,
                Data = await file.ReadAsBytesAsync()
            });
        }

        await _context.AddRangeAsync(documents, cancellationToken);

        var todo = await _context.FindAsync<Todo>(request.TodoId);
        todo.Documents = documents.Select(x => x.Id).ToList();

        await _context.SaveChangesAsync();

        return;
    }
}
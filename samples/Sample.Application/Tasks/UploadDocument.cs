using MediatR;
using MediatRpc;
using Sample.Application.Tasks.Types;
using System.Text.Json.Serialization;

namespace Sample.Application.Tasks;

[Command]
public record UploadDocument : IRequest, IFileRequest
{
    public Guid TaskId { get; init; }

    [JsonIgnore]
    public IReadOnlyList<IFile> Files { get; set; }
}
public class UploadDocumentHandler(ApplicationContext context) : IRequestHandler<UploadDocument>
{
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

        await context.AddRangeAsync(documents, cancellationToken);

        var taskItem = await context.Tasks.FindAsync(request.TaskId);
        taskItem.Documents = documents.Select(x => x.Id).ToList();

        await context.SaveChangesAsync(cancellationToken);
    }
}
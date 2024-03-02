namespace MediatRpc.Responses;
public record FileResponse : IResponse
{
    public required string Filename { get; set; }
    public required byte[] Data { get; set; }
    public required string ContentType { get; set; }
}

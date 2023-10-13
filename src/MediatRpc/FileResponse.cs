namespace MediatRpc;
public record FileResponse
{
    public string Filename { get; set; }
    public byte[] Data { get; set; }
    public string ContentType { get; set; }
}

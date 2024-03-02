namespace MediatRpc.Tools.ClientApi.Model;

public class ClientOperation
{
    public string Request { get; set; }
    public string Response { get; set; }
    public string ResponseType { get; set; }
    public string MethodName { get; set; }
    public string JsonRpcMethod { get; set; }
    public bool IsArray { get; set; }
    public bool IsFileRequest { get; set; }
    public bool IsFileResponse { get; set; }
    public bool IsParams { get; set; }
    public override string ToString()
        => MethodName;
}

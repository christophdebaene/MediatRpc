using System.Linq;

namespace MediatRpc;
public record RequestName(string? Namespace, string? ServiceName, string Name)
{
    public override string ToString()
    {
        string?[] parts = [Namespace, ServiceName, Name];
        return string.Join(".", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
    public static RequestName Parse(string name)
    {
        var parts = name.Split(".");
        return parts.Length switch
        {
            1 => new(null, null, parts[0]),
            2 => new(null, parts[0], parts[1]),
            _ => new(string.Join(".", parts[0..^3]), parts[^2], parts[^1]),
        };
    }
}
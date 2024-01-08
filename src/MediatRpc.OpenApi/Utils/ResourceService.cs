using System.Collections.Concurrent;
using System.Reflection;

namespace MediatRpc.OpenApi.Utils;
internal static class ResourceService
{
    private static readonly ConcurrentDictionary<string, string> _resources = new ConcurrentDictionary<string, string>();
    static ResourceService()
    {
        var thisAssembly = typeof(ResourceService).Assembly;

        foreach (var name in thisAssembly.GetManifestResourceNames())
            _resources.TryAdd(name.StripExtension(), thisAssembly.GetEmbeddedResourceString(name));
    }
    public static string Get(string name)
    {
        var paths = _resources.Keys
            .Where(x => x.EndsWith(name, StringComparison.InvariantCultureIgnoreCase))
            .ToArray();

        if (!paths.Any())
            throw new InvalidOperationException($"Resource ending with {name} not found.");

        if (paths.Length > 1)
            throw new InvalidOperationException($"Multiple resources ending with {name} found: {string.Join(Environment.NewLine, paths)}");

        return _resources[paths.Single()];
    }
    static string StripExtension(this string name) => name.Substring(0, name.LastIndexOf("."));
    static string GetEmbeddedResourceString(this Assembly assembly, string resourceFileName)
    {
        using (var stream = assembly.GetManifestResourceStream(resourceFileName))
        using (var streamReader = new StreamReader(stream))
            return streamReader.ReadToEnd();
    }
}

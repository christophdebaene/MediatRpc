using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatRpc.Metadata;
internal static class TypeExtensions
{
    public static IEnumerable<Type> ScanRequestTypes(this Assembly assembly)
    {
        return assembly
            .GetExportedTypes()
            .Where(x => !x.IsAbstract && !x.IsInterface && IsRequest(x));
    }
    public static IEnumerable<Type> ScanRequestHandlerTypes(this Assembly assembly, Type requestType)
    {
        foreach (var type in assembly.GetExportedTypes()
                                .Where(x => !x.IsAbstract && !x.IsInterface))
        {
            foreach (var interfaceType in type.GetInterfaces()
                                .Where(x => x.IsGenericType))
            {
                if (interfaceType.GetGenericArguments().First() == requestType &&
                    interfaceType.GetGenericArguments().Last() == GetResponseType(requestType))
                    yield return type;
            }
        }
    }
    public static bool IsRequest(this Type type)
    {
        return type.GetInterfaces().Any(x => x.FullName == "MediatR.IBaseRequest");
    }
    public static Type GetResponseType(this Type type)
    {
        var genericRequest = type.GetInterface("IRequest`1");
        if (genericRequest == null)
            return typeof(Unit);

        return genericRequest.GetGenericArguments().First();
    }
}
using System;

namespace MediatRpc;

public class CommandAttribute : RequestTypeAttribute
{
    public CommandAttribute() : base(RequestType.Command)
    {
    }
    public static bool Is(Type type) => Get(type) == RequestType.Command;
}

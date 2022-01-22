using System;

namespace EventHook.Abstractions
{
    public interface HookPointer
    {
        event EventHandler<(int x, int y)> Moved;
    }
}

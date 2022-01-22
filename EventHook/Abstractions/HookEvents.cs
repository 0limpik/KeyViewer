using System;

namespace EventHook.Abstractions
{
    internal interface HookEvents<Key>
    {
        event EventHandler<Key> Down;
        event EventHandler<Key> Up;
        event EventHandler<Key> Event;
    }
}

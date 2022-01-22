using System;

namespace EventHook.Abstractions
{
    public interface HookWindow
    {
        event EventHandler<WindowParameters> Changed;
    }

    public class WindowParameters
    {
        public string FilePath { get; set; }

        public string ModuleName { get; set; }

        public string ProcessName { get; set; }

        public string MainWindowTitle { get; set; }

        public DateTime StartTime { get; set; }
    }
}

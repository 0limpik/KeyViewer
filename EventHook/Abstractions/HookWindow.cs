using System;

namespace EventHook.Abstractions
{
    public interface HookWindow
    {
        event EventHandler<(WindowParameters from, WindowParameters to)> Changed;
    }

    public class WindowParameters
    {
        public string FilePath { get; set; }

        public string ModuleName { get; set; }

        public string ProcessName { get; set; }

        public string MainWindowTitle { get; set; }

        public DateTime StartTime { get; set; }

        public bool Equals(WindowParameters windowParameters)
        {
            return FilePath == windowParameters.FilePath;
        }
    }
}

using System;
using System.ComponentModel;
using EventHook.Abstractions;

namespace KeyViewer.Model.Programs
{
    public class ProgramSettings
    {
        public bool IsVisible { get; set; } = false;
        public WindowParameters Window { get; set; }

        public string DisplayName
        {
            get
            {
                switch (DisplayNameSource)
                {
                    case DisplayNameSource.Default: return Window?.ProcessName ?? Window?.ModuleName ?? Window?.MainWindowTitle;
                    case DisplayNameSource.ProcessName: return Window?.ProcessName;
                    case DisplayNameSource.ModuleName: return Window?.ModuleName;
                    case DisplayNameSource.Title: return Window?.MainWindowTitle;
                    default: return Window?.ProcessName ?? Window?.ModuleName ?? Window?.MainWindowTitle;
                }
            }
        }

        public DisplayNameSource DisplayNameSource { get; set; }

        public bool TopmostTimer { get; set; }

        public Alighument Alight { get; set; }
        public Point AlightPosition { get; set; } = new Point();

        public TimeSpan InGameAllTime { get; set; }
    }

    public class Point
    {
        public double X { get; set; }

        public double Y { get; set; }
    }

    public enum DisplayNameSource
    {
        [Description("Any (not null)")]
        Default = 0,

        [Description("Process Name (like Task Manager)")]
        ProcessName = 1,
        [Description("Module Name (like Task Manager in list)")]
        ModuleName = 2,

        [Description("Main Window Title")]
        Title = 3
    }

    public enum Alighument
    {
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
    }
}

using System;
using System.ComponentModel;
using EventHook.Abstractions;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.Model.Programs
{
    public class ProgramSettings : AViewModel
    {
        public WindowParameters Window { get; set; }

        [AlsoNotifyFor(nameof(DisplayName))]
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

        [AlsoNotifyFor(nameof(DisplayName))]
        public DisplayNameSource DisplayNameSource { get; set; }

        public bool TopmostTimer { get; set; }
        public bool IsVisible { get; set; } = false;

        public Alighument Alight { get; set; }
        public Point AlightPosition { get; set; } = new Point();

        [AlsoNotifyFor(nameof(InGameAllTime))]
        public TimeSpan InGameAllTime { get; set; }
    }

    public class Point : AViewModel
    {
        [AlsoNotifyFor(nameof(X))]
        public double X { get; set; }

        [AlsoNotifyFor(nameof(Y))]
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

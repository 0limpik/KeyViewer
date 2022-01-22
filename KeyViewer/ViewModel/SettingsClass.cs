using System.Windows;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel
{
    public class SettingsClass : AViewModel
    {
        public static double SizeX { get; set; } = 750;
        public static double SizeY { get; set; } = 175;

        public static double ScreenSizeX => SystemParameters.WorkArea.Width;
        public static double ScreenSizeY => SystemParameters.WorkArea.Height;

        public static bool IsInDesignMode()
        {
            if (System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio"))
            {
                return true;
            }
            return false;
        }
    }
}

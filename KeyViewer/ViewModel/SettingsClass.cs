using System.Windows;
using KeyViewer.Model.Programs;
using KeyViewer.ViewModel.Settings;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;
using Point = KeyViewer.Model.Programs.Point;

namespace KeyViewer.ViewModel
{
    public class SettingsClass : AViewModel
    {
        public static double SizeX { get; set; } = 750;
        public static double SizeY { get; set; } = 175;

        public static double ScreenSizeX => SystemParameters.WorkArea.Width;
        public static double ScreenSizeY => SystemParameters.WorkArea.Height;

        public static Point GetPoint(Point point, Alighument alighument)
        {
            switch (alighument)
            {
                case Alighument.LeftTop:
                    return new Point() { X = point.X, Y = point.Y };
                case Alighument.LeftBottom:
                    return new Point() { X = point.X, Y = SettingsClass.ScreenSizeY - SettingsClass.SizeY - point.Y };
                case Alighument.RightTop:
                    return new Point() { X = SettingsClass.ScreenSizeX - SettingsClass.SizeX - point.X, Y = point.Y };
                case Alighument.RightBottom:
                    return new Point() { X = SettingsClass.ScreenSizeX - SettingsClass.SizeX - point.X, Y = SettingsClass.ScreenSizeY - SettingsClass.SizeY - point.Y };
                default:
                    return new Point();
            }
        }

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

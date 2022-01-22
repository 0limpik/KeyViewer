using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyViewer.View.Converters
{
    public class TimeCutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                return ConvertHelper.CutTime(time);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeToNowCutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime time)
            {
                var deltaTime = DateTime.Now - time;

                return ConvertHelper.CutTime(deltaTime);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeShortCutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                return ConvertHelper.ShortCutTime(time);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SecondMilisecondConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                return $"{(int)time.TotalSeconds}.{time.Milliseconds / 100}";
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class ConvertHelper
    {
        public static string CutTime(TimeSpan time)
        {
            if (time.TotalMinutes < 60)
                return $"{(int)time.TotalMinutes} minutes";

            return $"{(int)time.TotalHours}.{time.Minutes / 10} hours";
        }
        public static string ShortCutTime(TimeSpan time)
        {
            if (time.TotalHours < 10)
                return $"{(int)time.TotalHours}.{time.Minutes:D2}";

            if (time.TotalHours < 100)
                return $"{(int)time.TotalHours}.{time.Minutes / 10}";

            return $"{(int)time.TotalHours}";
        }
    }
}

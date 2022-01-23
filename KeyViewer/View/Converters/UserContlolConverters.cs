using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KeyViewer.View.Converters
{

    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, 0, 0, System.Convert.ToDouble(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool visible)
            {
                return UserContlolConverters.BoolToHidden(visible);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EqualsVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool.TryParse(parameter?.ToString(), out bool inverse);

            return UserContlolConverters.BoolToHidden(values[0] == values[1], inverse);

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class UserContlolConverters
    {
        public static Visibility BoolToHidden(bool value, bool inverse = false)
            => inverse
            ? value ? Visibility.Hidden : Visibility.Visible
            : value ? Visibility.Visible : Visibility.Hidden;

        public static Visibility BoolToCollapsed(bool value, bool inverse = false)
            => inverse
            ? value ? Visibility.Collapsed : Visibility.Visible
            : value ? Visibility.Visible : Visibility.Collapsed;
    }
}

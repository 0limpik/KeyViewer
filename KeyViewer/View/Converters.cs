using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using KeyViewer.ViewModel;

namespace KeyViewer.View
{
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, 0, 0, System.Convert.ToDouble(value));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class MaxTextLenghtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value?.ToString();

            if (text != null)
            {
                int.TryParse(parameter?.ToString(), out int maxLenght);

                if (text.Length > maxLenght)
                    return value?.ToString().Substring(0, maxLenght) + "...";
            }
            return text;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class IsActiveChanged : IValueConverter
    {
        class PrevKey
        {
            public string Name;
            public bool IsActive;
        }

        List<PrevKey> recent = new List<PrevKey>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Key key = (Key)value;

            var ret = parameter.ToString() == "True";

            var prev = recent.FirstOrDefault(x => x.Name == key.Name);

            if (prev == null)
            {
                prev = new PrevKey { Name = key.Name, IsActive = key.IsActive };
                recent.Add(prev);
            }

            System.Diagnostics.Trace.WriteLine($"{key.Name} : {key.IsActive}, {prev.IsActive}");

            if (key.IsActive != prev.IsActive)
            {
                prev.IsActive = key.IsActive;
                return false;
            }

            return key.IsActive;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsFirstItem : IMultiValueConverter
    {
        List<(string Name, bool IsActive)> list = new List<(string name, bool IsActive)>();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Key key = (Key)values[0];
            Key lastKey = (Key)values[1];

            System.Diagnostics.Trace.WriteLine($"{key.Name} : {key.IsActive}, {lastKey.Name} : {lastKey.IsActive} ? {(key == lastKey ? key.IsActive : !key.IsActive)}");

            if (key.Name == "Ctrl" && key != lastKey)
                return false;
            return key == lastKey ? key.IsActive : !key.IsActive;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumDescriptionConverter : IValueConverter
    {
        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
                return attrib.Description;
            }
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum myEnum = (Enum)value;
            string description = GetEnumDescription(myEnum);
            return description;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
    public class MathRoundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Round(double.Parse(value.ToString()), int.Parse(parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool visible)
            {
                return visible ? Visibility.Visible : Visibility.Hidden;
            }
            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
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

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
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

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
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

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
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

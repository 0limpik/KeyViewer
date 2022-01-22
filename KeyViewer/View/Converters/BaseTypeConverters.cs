using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using KeyViewer.ViewModel.Keys;

namespace KeyViewer.View.Converters
{
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }

    public class EnumDescriptionConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(string.IsNullOrEmpty(value.ToString()))
                return null;

            if (value is Enum myEnum)
            {
                return GetEnumDescription(myEnum);
            }

            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetEnumDescription(Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

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
    }

    public class IsFirstItem : IMultiValueConverter
    {
        List<(string Name, bool IsActive)> list = new List<(string name, bool IsActive)>();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            KeyItem key = (KeyItem)values[0];
            KeyItem lastKey = (KeyItem)values[1];

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
}

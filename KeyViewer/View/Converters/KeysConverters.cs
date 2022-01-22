using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using KeyViewer.ViewModel.Keys;

namespace KeyViewer.View.Converters
{
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
            KeyItem key = (KeyItem)value;

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
}

using AutoSettings.Model.Attributes;
using PropertyChanged;

namespace AutoSettings.Model.SettingTypes
{
    public class IntSetting : ValueSetting
    {
        public int Value { get; set; }
        public int DefaultValue { get; set; }
    }
}

using System.Reflection;
using AutoSettings.Model.SettingTypes;

namespace AutoSettings.Model.Attributes
{
    public class StringSettingAttribute : ValueSettingAttribute
    {
        public string DefaultValue { get; set; }

        public override object Default => (object)DefaultValue;

        public StringSettingAttribute(string name, string description, string defaultValue)
            : base(name, description)
        {
            DefaultValue = defaultValue;
        }

        public override ValueSetting GetModel(PropertyInfo property, object settings)
            => new StringSetting
            {
                Name = Name,
                Description = Description,
                Value = property.GetValue(settings).ToString(),
                DefaultValue = DefaultValue
            };
    }
}

using System.Reflection;
using AutoSettings.Model.SettingTypes;

namespace AutoSettings.Model.Attributes
{
    public class BoolSettingAttribute : ValueSettingAttribute
    {
        public bool DefaultValue { get; set; }

        public override object Default => (object)DefaultValue;

        public BoolSettingAttribute(string name, string description, string defaultValue)
            : base(name, description)
        {
            if (bool.TryParse(defaultValue, out bool outputValue))
            {
                DefaultValue = outputValue;
            }
        }

        public override ValueSetting GetModel(PropertyInfo property, object settings)
            => new BoolSetting
            {
                Name = Name,
                Description = Description,
                Value = (bool)property.GetValue(settings),
                DefaultValue = DefaultValue
            };
    }
}

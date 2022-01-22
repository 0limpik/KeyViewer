using System.Reflection;
using AutoSettings.Model.SettingTypes;

namespace AutoSettings.Model.Attributes
{
    public class IntSettingAttribute : ValueSettingAttribute
    {
        public int DefaultValue;
        public override object Default => (object)DefaultValue;

        public IntSettingAttribute(string name, string description, string defaultValue)
            : base(name, description)
        {
            if (int.TryParse(defaultValue, out int outputValue))
            {
                DefaultValue = outputValue;
            }
        }

        public override ValueSetting GetModel(PropertyInfo property, object settings)
           => new IntSetting
           {
               Name = Name,
               Description = Description,
               Value = (int)property.GetValue(settings),
               DefaultValue = DefaultValue
           };

    }
}

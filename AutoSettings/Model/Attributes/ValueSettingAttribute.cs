using System;
using System.Reflection;
using AutoSettings.Model.SettingTypes;

namespace AutoSettings.Model.Attributes
{
    public abstract class ValueSettingAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public abstract object Default { get; }

        public ValueSettingAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract ValueSetting GetModel(PropertyInfo property, object settings);
    }
}

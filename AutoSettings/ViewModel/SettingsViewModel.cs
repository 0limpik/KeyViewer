using System.Collections.Generic;
using System.Reflection;
using AutoSettings.Model.Attributes;
using AutoSettings.Model.SettingTypes;
using AutoSettings.Services;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace AutoSettings.ViewModel
{
    public class SettingsViewModel : AViewModel
    {
        [AlsoNotifyFor(nameof(Settings))]
        public List<ValueSetting> Settings { get; set; } = new List<ValueSetting>();

        public SettingsViewModel()
        {
            var example = new ExampleSettingRepository();

            foreach (var item in example.Model.GetType().GetProperties())
            {
                var test = GetAttribute(item);
                var test2 = test.GetModel(item, example.Model);
                Settings.Add(test2);
            }
        }

        private ValueSettingAttribute GetAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<ValueSettingAttribute>();
        }
    }
}

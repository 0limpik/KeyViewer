using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using AutoSettings.Model.Attributes;
using PropertyChanged;

namespace AutoSettings.Model.SettingTypes
{
    public class StringSetting : ValueSetting
    {
        public string Value { get; set; }
        public string DefaultValue { get; set; }
    }
}


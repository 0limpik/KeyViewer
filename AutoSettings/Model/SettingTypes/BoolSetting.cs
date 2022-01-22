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
    public class BoolSetting : ValueSetting
    {
        public bool Value { get; set; }
        public bool DefaultValue { get; set; }
    }
}

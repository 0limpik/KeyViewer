using System;

namespace AutoSettings.Model
{
    public class ValueSettingModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Type ValueType { get; set; }
        public object Value { get; set; }
        public object DefaultValue { get; set; }
    }
}

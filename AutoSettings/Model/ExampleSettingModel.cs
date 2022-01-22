using System;
using AutoSettings.Model.Attributes;

namespace AutoSettings.Model
{
    public class ExampleSettingModel
    {
        [BoolSetting(nameof(IsEnabled), "Включено ли", "false")]
        public bool IsEnabled { get; set; }

        [IntSetting(nameof(Count), "Количество", "99")]
        public int Count { get; set; }

        [IntSetting(nameof(Count2), "Количество номер 2", "-15")]
        public int Count2 { get; set; }

        [StringSetting(nameof(test), "Строка", "АыАы")]
        public string test { get; set; }
    }
}

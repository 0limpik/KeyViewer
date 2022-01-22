using System;
using System.IO;
using System.Reflection;
using AutoSettings.Model;
using AutoSettings.Model.Attributes;
using Newtonsoft.Json;

namespace AutoSettings.Services
{
    public class ExampleSettingRepository
    {
        public ExampleSettingModel Model;

        public ExampleSettingRepository()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {
                Init();
            }
            Save();
        }

        void Init()
        {
            Model = new ExampleSettingModel();

            foreach (var item in Model.GetType().GetProperties())
            {
                item.SetValue(Model, GetDefaultValue(item));
            }
        }

        void Load()
        {
            var loadRepository = JsonConvert.DeserializeObject<ExampleSettingModel>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\test.json"));
            Model = loadRepository ?? throw new Exception("Ошибка при загрузке");
        }
        void Save()
        {
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\test.json", JsonConvert.SerializeObject(Model, Formatting.Indented));
        }

        private object GetDefaultValue(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<ValueSettingAttribute>();

            return attr.Default;
        }
    }
}

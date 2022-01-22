using System;
using System.IO;
using System.Linq;
using EventHook;
using KeyViewer.Services.Implementations;
using KeyViewer.ViewModel;

namespace KeyViewer.Services
{
    internal class KeyInfoRepository<T> : JsonRepository<KeyParameters<T>> where T : Enum
    {
        public KeyInfoRepository(string RepositoryName) : base("KeyInfos", RepositoryName) { }

        public override void Init()
        {
            Directory.CreateDirectory(RepositoryDirectory);
            Repository = Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Select(x =>
                    {
                        var key = new KeyParameters<T> { Key = x, Name = x.ToString() };

                        var atr = x.GetAttributeOfType<KeyAttribute>();

                        key.DisplayName = atr.Name;
                        key.Description = atr.Description;

                        return key;
                    })
                    .ToList();
        }
    }

    public class KeyParameters<T> where T : Enum
    {
        public T Key { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Linq;
using EventHook;
using EventHook.Implementations.Keyboard;
using EventHook.Implementations.Mouse;
using KeyViewer.Extensions;
using KeyViewer.Services.Implementations;
using KeyViewer.ViewModel;

namespace KeyViewer.Services
{
    public class KeyboardKeysRepository : KeyInfoRepository<KeyboardKeys>
    {
        public static KeyboardKeysRepository Instanse => _Instanse == null ? _Instanse = new KeyboardKeysRepository() : _Instanse;
        private static KeyboardKeysRepository _Instanse;

        private KeyboardKeysRepository() : base("KeyboardRepository.json") { }
    }
    public class MouseKeysRepository : KeyInfoRepository<MouseKeys>
    {
        public static MouseKeysRepository Instanse => _Instanse == null ? _Instanse = new MouseKeysRepository() : _Instanse;
        private static MouseKeysRepository _Instanse;

        private MouseKeysRepository() : base("MouseRepository.json") { }
    }

    public class KeyInfoRepository<T> : JsonRepository<KeyParameters<T>> where T : Enum
    {
        public KeyInfoRepository(string RepositoryName) : base("KeyInfos", RepositoryName) { }

        public override void Init()
        {
            Repository = Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(x =>
                {
                    var atr = x.GetAttributeOfType<KeyAttribute>();

                    if (!atr.Diplay) return null;

                    return new KeyParameters<T>
                    {
                        Key = x,
                        Name = x.ToString(),
                        DisplayName = atr.Name,
                        Description = atr.Description
                    };
                })
                .Where(x => x != null)
                .ToList();
            Save();
        }
    }

    public class KeyParameters<T> where T : Enum
    {
        public T Key { get; set; }

        public string Name { get; set; }

        public string DisplayName { get => _DisplayName != null ? _DisplayName : Key.ToString() ; set => _DisplayName = value; }
        private string _DisplayName;

        public string Description { get; set; }
    }
}

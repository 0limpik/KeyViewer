using System.ComponentModel;
using System.Windows.Data;
using EventHook.Implementations.Keyboard;
using EventHook.Implementations.Mouse;
using KeyViewer.Services;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel.Settings
{
    public class KeysEditorViewModel : AViewModel
    {
        public ICollectionView KeyboardKeys => _KeyboardKeys.View;
        private CollectionViewSource _KeyboardKeys = new CollectionViewSource();

        public ICollectionView MouseKeys => _MouseKeys.View;
        private CollectionViewSource _MouseKeys = new CollectionViewSource();

        [AlsoNotifyFor(nameof(KeyFilter))] public string KeyFilter { 
            get => _KeyFilter;
            set
            {
                if(_KeyFilter != value)
                {
                    _KeyFilter = value;
                    KeyboardKeys.Refresh();
                    MouseKeys.Refresh();
                }
            } 
        }
        private string _KeyFilter;

        private bool KeysFilter(object sender)
        {
            if(string.IsNullOrEmpty(KeyFilter))
                return true;

            var filter = KeyFilter.ToLower();

            if (sender is KeyParameters<KeyboardKeys> keyboardParameters)
            {
                return keyboardParameters.Name.ToLower().Contains(filter)
                   || keyboardParameters.DisplayName.ToLower().Contains(filter)
                   || keyboardParameters.Description.ToLower().Contains(filter);
            }

            if (sender is KeyParameters<MouseKeys> mouseParameters)
            {
                return mouseParameters.Name.ToLower().Contains(filter)
                   || mouseParameters.DisplayName.ToLower().Contains(filter)
                   || mouseParameters.Description.ToLower().Contains(filter);
            }

            return false;
        }

        private KeyboardKeysRepository keyboardRepository = KeyboardKeysRepository.Instanse;
        private MouseKeysRepository mouseRepository = MouseKeysRepository.Instanse;

        public KeysEditorViewModel()
        {
            _KeyboardKeys.Source = keyboardRepository.Repository;
            KeyboardKeys.Filter = KeysFilter;

            _MouseKeys.Source = mouseRepository.Repository;
            MouseKeys.Filter = KeysFilter;

            PropertyChanged += KeysEditorViewModel_PropertyChanged;
        }


        private void KeysEditorViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            keyboardRepository.Save();
            mouseRepository.Save();
        }
    }

    public class KeyboardParameters : KeyParameters<KeyboardKeys> { }
    public class MouseParameters : KeyParameters<MouseKeys> { }
}

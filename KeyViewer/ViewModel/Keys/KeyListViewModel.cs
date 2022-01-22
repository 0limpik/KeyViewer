using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using KeyViewer.Extensions;

namespace KeyViewer.ViewModel.Keys
{
    public class KeyListViewModel
    {
        private List<KeyItem> KeysCollection = new List<KeyItem>();

        private DispatcherTimer timer;

        public Action<KeyItem> AddKeyAction;
        public Action<KeyItem> RemoveKeyAction;

        public readonly int UpdateRatePerSecond = 60;
        public readonly TimeSpan DisappeareTime = TimeSpan.FromMilliseconds(1500);

        public KeyListViewModel()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000 / UpdateRatePerSecond)
            };
            timer.Tick += Update;
            timer.Start();
        }

        public KeyListViewModel Add(string keyName)
        {
            var keyItem = KeysCollection.FirstOrDefault(x => x.Name == keyName);

            if (keyItem == null)
            {
                keyItem = new KeyItem(keyName);
                KeysCollection.Add(keyItem);
                AddKeyAction?.Invoke(keyItem);
            }

            keyItem.Activate();

            return this;
        }

        public KeyListViewModel Remove(string keyName)
        {
            KeysCollection.FirstOrDefault(x => x.Name == keyName)?.Deactivate();

            return this;
        }

        private void Update(object sender, EventArgs e)
        {
            List<KeyItem> toRemove = new List<KeyItem>();

            foreach (var key in KeysCollection)
            {
                if (!key.IsActive && key.DeactivateTime < DateTime.Now - DisappeareTime)
                {
                    toRemove.Add(key);
                    RemoveKeyAction?.Invoke(key);
                }

                if (key.IsActive)
                {
                    key.OnPropertyChanged(nameof(key.Time));
                }

                var color = key.Palette.GetRelativeColor(GetColorOffset(key.Count, (int)key.Time.TotalMilliseconds));
                key.Brush = new SolidColorBrush(color);
            }

            foreach (var key in toRemove)
            {
                KeysCollection.Remove(key);
            }
        }

        private double GetColorOffset(int count, int millisecond)
        {
            double offset = (count / 100.0) + (millisecond / 30000.0);
            return offset > 1.0 ? 1.0 : offset;
        }
    }
}

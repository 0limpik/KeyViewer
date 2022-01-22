using System;
using System.Collections.Generic;
using System.Windows.Media;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel.Keys
{
    public class KeyItem : AViewModel
    {
        public string Name { get; set; }
        [AlsoNotifyFor(nameof(IsActive))] public bool IsActive { get; set; }
        [AlsoNotifyFor(nameof(Brush))] public Brush Brush { get; set; } = Brushes.White;
        [AlsoNotifyFor(nameof(Count))] public int Count { get; set; }

        [AlsoNotifyFor(nameof(Time))] public TimeSpan Time => RawTime + (DateTime.Now - ActivateTime);
        private TimeSpan RawTime;

        public GradientStopCollection Palette { get; private set; } = new GradientStopCollection(new List<GradientStop>()
        {
            new GradientStop(Colors.White, 0),
            new GradientStop(Colors.Lime, 0.25),
            new GradientStop(Colors.RoyalBlue, 0.50),
            new GradientStop(Colors.Orchid, 0.75),
            new GradientStop(Colors.Orange, 1.0),
        });

        public KeyItem(string Name)
        {
            this.Name = Name;
        }

        public DateTime ActivateTime { get; private set; }
        public void Activate()
        {
            IsActive = true;
            ActivateTime = DateTime.Now;

            Count++;
        }

        public DateTime DeactivateTime { get; private set; }
        public void Deactivate()
        {
            IsActive = false;
            DeactivateTime = DateTime.Now;
            RawTime += DateTime.Now - ActivateTime;
        }
    }
}

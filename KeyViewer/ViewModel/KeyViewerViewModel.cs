using System;
using System.Linq;
using System.Windows.Threading;
using EventHook.Abstractions;
using EventHook.Implementations.Keyboard;
using EventHook.Implementations.Mouse;
using EventHook.Implementations.Window;
using KeyViewer.Extensions;
using KeyViewer.Model.Programs;
using KeyViewer.Services;
using KeyViewer.ViewModel.Keys;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;

namespace KeyViewer.ViewModel
{
    public class KeyViewerViewModel : AViewModel
    {
        [AlsoNotifyFor(nameof(MouseAngle))] public double MouseAngle { get; set; }

        [AlsoNotifyFor(nameof(CurrentWindow), nameof(WindowName), nameof(WindowTime))]
        public ProgramSettings CurrentWindow { get; set; } = new ProgramSettings();

        [AlsoNotifyFor(nameof(Left))] public double Left { get; set; }
        [AlsoNotifyFor(nameof(Top))] public double Top { get; set; }
        public string WindowName => CurrentWindow.DisplayName;
        public string WindowTime => $"{CurrentWindow.InGameAllTime + (DateTime.Now - LastWindowChanged):h\\:mm\\:ss}";

        public string SystemTime => $"{DateTime.Now:h:mm:ss}";
        public string SystemDate => $"{DateTime.Now:MMM/dd tt}";

        public KeyListViewModel keyListViewModel { get; set; } = new KeyListViewModel();

        private WindowHook windowHook = new WindowHook();
        private ProgramsRepository programsRepository = ProgramsRepository.Instanse;
        private DeletedProgramsRepository deletedProgramsRepository = DeletedProgramsRepository.Instanse;

        private KeyboardHook keyboardHook = new KeyboardHook();
        private KeyboardKeysRepository keyboardRepository = KeyboardKeysRepository.Instanse;

        private MouseHook mouseHook = new MouseHook();
        private MouseKeysRepository mouseRepository = MouseKeysRepository.Instanse;

        private DispatcherTimer timer;
        private DateTime LastWindowChanged = DateTime.Now;

        private ProgramSettings PrevWindow;

        public KeyViewerViewModel()
        {
            if (!SettingsClass.IsInDesignMode()) windowHook.Changed += WindowUpdate;

            keyboardHook.Down += (s, key) => InvokeIsVisible(() => keyListViewModel.Add(key.DisplayName(keyboardRepository.Repository)));
            keyboardHook.Up += (s, key) => keyListViewModel.Remove(key.DisplayName(keyboardRepository.Repository));

            mouseHook.Down += (s, key) => InvokeIsVisible(() => keyListViewModel.Add(key.DisplayName(mouseRepository.Repository)));
            mouseHook.Up += (s, key) => keyListViewModel.Remove(key.DisplayName(mouseRepository.Repository));
            mouseHook.Event += (s, key) => InvokeIsVisible(() => keyListViewModel.Add(key.DisplayName(mouseRepository.Repository)).Remove(key.DisplayName(mouseRepository.Repository)));

            mouseHook.Moved += MouseHook_Moved;

            keyboardHook.Hook();
#if !DEBUG
            mouseHook.Hook();
#endif
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000 / 60)
            };
            timer.Tick += Update;
            timer.Start();
        }

        private void InvokeIsVisible(Action action)
        {
            if (CurrentWindow.IsVisible)
            {
                action?.Invoke();
            }
        }

        private void WindowUpdate(object sender, WindowParameters window)
        {
            System.Diagnostics.Trace.WriteLine(window.FilePath);
            if (deletedProgramsRepository.Repository.Contains(window.FilePath))
            {
                CurrentWindow = new ProgramSettings { IsVisible = false };
                return;
            };

            var programSettings = programsRepository.Repository.FirstOrDefault(x => CompareWindow(x, window));

            if (programSettings == null)
            {
                programSettings = programsRepository.GetDefault();
                programSettings.Window = window;
                programsRepository.Repository.Add(programSettings);
            }

            PrevWindow = CurrentWindow;

            programSettings.Window = window;
            CurrentWindow = programSettings;

            if (CurrentWindow.IsVisible)
            {
                var point = SettingsClass.GetPoint(CurrentWindow.AlightPosition, CurrentWindow.Alight);
                Left = point.X;
                Top = point.Y;
            }

            PrevWindow.InGameAllTime += DateTime.Now - LastWindowChanged;
            programsRepository.Save();

            LastWindowChanged = DateTime.Now;
        }

        private void Update(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(WindowTime));
            OnPropertyChanged(nameof(SystemTime));
            OnPropertyChanged(nameof(SystemDate));
        }

        public void Exit()
        {
            keyboardHook?.Unhook();
            mouseHook?.Unhook();
            programsRepository.Save();
        }

        private DateTime LastMouseMove;
        private (int x, int y) LastMouseCords;

        private void MouseHook_Moved(object sender, (int x, int y) e)
        {
            if (DateTime.Now - LastMouseMove > TimeSpan.FromMilliseconds(150))
            {
                MouseAngle = GetAngle(e, LastMouseCords);

                LastMouseCords = e;
                LastMouseMove = DateTime.Now;
            }
        }

        private static double GetAngle((int x, int y) start, (int x, int y) end)
        {
            return Math.Atan2(start.y - end.y, start.x - end.x) / Math.PI * 180;
        }

        private bool CompareWindow(ProgramSettings x, WindowParameters y)
        {
            if (x.Window.FilePath == y.FilePath)
                return true;

            return false;
        }
    }
}

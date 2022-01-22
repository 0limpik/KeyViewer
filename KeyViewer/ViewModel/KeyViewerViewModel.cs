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
using Point = KeyViewer.Model.Programs.Point;

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

        private Point GetPoint(Point point, Alighument alighument)
        {
            switch (alighument)
            {
                case Alighument.LeftTop:
                    return new Point() { X = point.X, Y = point.Y };
                case Alighument.LeftBottom:
                    return new Point() { X = point.X, Y = SettingsClass.ScreenSizeY - SettingsClass.SizeY - point.Y };
                case Alighument.RightTop:
                    return new Point() { X = SettingsClass.ScreenSizeX - SettingsClass.SizeX - point.X, Y = point.Y };
                case Alighument.RightBottom:
                    return new Point() { X = SettingsClass.ScreenSizeX - SettingsClass.SizeX - point.X, Y = SettingsClass.ScreenSizeY - SettingsClass.SizeY - point.Y };
                default:
                    return new Point();
            }
        }
        public KeyListViewModel keyListViewModel { get; set; } = new KeyListViewModel();

        private WindowHook windowHook = new WindowHook();
        private ProgramsRepository programsRepository = ProgramsRepository.Instanse;

        private KeyboardHook keyboardHook = new KeyboardHook();
        private KeyInfoRepository<KeyboardKeys> keyboardRepository = new KeyInfoRepository<KeyboardKeys>("KeyboardRepository.json");

        private MouseHook mouseHook = new MouseHook();
        private KeyInfoRepository<MouseKeys> mouseRepository = new KeyInfoRepository<MouseKeys>("MouseRepository.json");

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
            var programSettings = programsRepository.Repository.FirstOrDefault(x => CompareWindow(x, window));

            if (programSettings == null)
            {
                programSettings = new ProgramSettings()
                {
                    Window = window
                };
                programsRepository.Repository.Add(programSettings);
            }

            PrevWindow = CurrentWindow;

            programSettings.Window = window;
            CurrentWindow = programSettings;

            if (CurrentWindow.IsVisible)
            {
                var point = GetPoint(CurrentWindow.AlightPosition, CurrentWindow.Alight);
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

    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}

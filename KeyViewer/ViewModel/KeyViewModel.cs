using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using EventHook.Abstractions;
using EventHook.Implementations.Keyboard;
using EventHook.Implementations.Mouse;
using EventHook.Implementations.Window;
using KeyViewer.Model.Programs;
using KeyViewer.Services;
using PropertyChanged;
using WPFMVVM.ViewModel.Abstractions;
using Point = KeyViewer.Model.Programs.Point;

namespace KeyViewer.ViewModel
{
    public class SettingsClass : AViewModel
    {
        public static double SizeX { get; set; } = 750;
        public static double SizeY { get; set; } = 175;

        public static double ScreenSizeX => SystemParameters.WorkArea.Width;
        public static double ScreenSizeY => SystemParameters.WorkArea.Height;
    }

    public class KeyViewModel : AViewModel
    {
        public KeyViewModel Instance => this;

        [AlsoNotifyFor(nameof(MouseAngle))] public double MouseAngle { get; set; }

        [AlsoNotifyFor(nameof(CurrentWindow), nameof(WindowName), nameof(WindowTime))]
        public ProgramSettings CurrentWindow { get; set; } = new ProgramSettings();
        public string WindowName => CurrentWindow?.DisplayName;
        [AlsoNotifyFor(nameof(WindowTime))] public string WindowTime { get; set; } = "0:00:00";
        [AlsoNotifyFor(nameof(SystemTime))] public string SystemTime { get; set; } = "0:00:00";
        [AlsoNotifyFor(nameof(SystemDate))] public string SystemDate { get; set; } = "0:00:00";

        [AlsoNotifyFor(nameof(Left))] public double Left { get; set; }
        [AlsoNotifyFor(nameof(Top))] public double Top { get; set; }

        private Point GetPoint(Point point, Alighument alighument)
        {
            System.Diagnostics.Trace.WriteLine($"{alighument}, {point.X} - {point.Y}");
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

        private WindowHook windowHook = new WindowHook();
        public ProgramsRepository programsRepository = ProgramsRepository.Instanse;

        private KeyboardHook keyboardHook = new KeyboardHook();
        private KeyInfoRepository<KeyboardKeys> keyboardRepository = new KeyInfoRepository<KeyboardKeys>("KeyboardRepository.json");

        private MouseHook mouseHook = new MouseHook();
        private KeyInfoRepository<MouseKeys> mouseRepository = new KeyInfoRepository<MouseKeys>("MouseRepository.json");

        KeyViewRepository keyViewRepository;
        public Action<Key> AddKey;
        public Action<Key> RemoveKey;

         
        DispatcherTimer timer;
        private DateTime LastWindowChanged = DateTime.Now;

        public KeyViewModel()
        {
            keyViewRepository = new KeyViewRepository((key) => RemoveKey?.Invoke(key));

            windowHook.Changed += (s, w) =>
            {
                CheckNewElement(w);

                var toProgram = programsRepository.Repository.First(x => x.Window.Equals(w.to));

                WindowTime = toProgram.InGameAllTime.ToString(@"h\:mm\:ss");

                toProgram.Window = w.to;

                CurrentWindow = toProgram;
                var point = GetPoint(CurrentWindow.AlightPosition, CurrentWindow.Alight); 
                Left = point.X;
                Top = point.Y;

                var fromProgram = programsRepository.Repository.First(x => x.Window.Equals(w.from));
                fromProgram.InGameAllTime += DateTime.Now - LastWindowChanged;

                LastWindowChanged = DateTime.Now;
            };

            keyboardHook.Down +=
                (s, keyEnum) =>
                {
                    var key = CreateKey(keyEnum, keyboardRepository);
                    if (keyViewRepository.Add(key))
                        AddKey?.Invoke(key);
                };
            keyboardHook.Up +=
                    (s, keyEnum) =>
                {
                    var key = CreateKey(keyEnum, keyboardRepository);
                    keyViewRepository.Remove(key);
                };
            keyboardHook.Hook();

            mouseHook.Down +=
                (s, keyEnum) =>
                {
                    var key = CreateKey(keyEnum, mouseRepository);
                    if (keyViewRepository.Add(key))
                        AddKey?.Invoke(key);
                };
            mouseHook.Up +=
                (s, keyEnum) =>
                {
                    var key = CreateKey(keyEnum, mouseRepository);
                    keyViewRepository.Remove(key);
                };
            mouseHook.Event +=
                (s, keyEnum) =>
                {
                    var key = CreateKey(keyEnum, mouseRepository);
                    if (keyViewRepository.Add(key))
                        AddKey?.Invoke(key);
                    keyViewRepository.Remove(key);
                };

            mouseHook.Moved += MouseHook_Moved;

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

        private void Update(object sender, EventArgs e)
        {
            var inGameTime = CurrentWindow?.InGameAllTime ?? TimeSpan.Zero;
            var delteTime = DateTime.Now - LastWindowChanged;
            WindowTime = (inGameTime + delteTime).ToString(@"h\:mm\:ss");
            SystemTime = DateTime.Now.ToString(@"h\:mm\:ss");
            SystemDate = DateTime.Now.ToString(@"MMM/dd tt");
        }

        public void Exit()
        {
            keyboardHook?.Unhook();
            mouseHook?.Unhook();
            programsRepository.Save();
        }

        private void CheckNewElement((WindowParameters from, WindowParameters to) param)
        {
            if (!programsRepository.Repository.Any(x => x.Window.Equals(param.from)))
            {
                programsRepository.Add(CreateFromWindowParam(param.from));
            }

            if (!programsRepository.Repository.Any(x => x.Window.Equals(param.to)))
            {
                programsRepository.Add(CreateFromWindowParam(param.to));
            }

            ProgramSettings CreateFromWindowParam(WindowParameters windowParameters)
                => new ProgramSettings
                {
                    Window = windowParameters,
                    InGameAllTime = TimeSpan.Zero,
                };
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

        private Key CreateKey<T>(T keyEnum, KeyInfoRepository<T> repository) where T : Enum
        {
            var repositoryKey = repository.Repository.FirstOrDefault(x => x.Key.Equals(keyEnum));

            var key = new Key(repositoryKey?.DisplayName ?? keyEnum.ToString());
            return key;
        }
    }

    public class KeyViewRepository
    {
        private List<Key> KeysCollection = new List<Key>();

        DispatcherTimer timer;
        Action<Key> removeAction;

        public readonly int UpdateRatePerSecond = 60;
        public readonly TimeSpan DisappeareTime = TimeSpan.FromMilliseconds(1500);

        public KeyViewRepository(
            Action<Key> removeAction)
        {
            this.removeAction = removeAction;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000 / UpdateRatePerSecond)
            };
            timer.Tick += Update;
            timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Была ли добавлена клавиша</returns>
        public bool Add(Key key)
        {
            Func<Key, bool> predicate = x => x.Name == key.Name;

            if (!KeysCollection.Any(predicate))
            {
                KeysCollection.Add(key);
                key.Activate();
                return true;
            }
            else
            {
                KeysCollection.FirstOrDefault(predicate)?.Activate();
                return false;
            }
        }

        public void Remove(Key key)
        {
            Func<Key, bool> predicate = x => x.Name == key.Name;

            KeysCollection.FirstOrDefault(predicate)?.Deactivate();
        }

        private void Update(object sender, EventArgs e)
        {
            List<Key> toRemove = new List<Key>();

            foreach (var key in KeysCollection)
            {
                if (!key.IsActive && key.DeactivateTime < DateTime.Now - DisappeareTime)
                {
                    removeAction.Invoke(key);
                    toRemove.Add(key);
                }
                if (key.IsActive)
                {
                    key.Time = $"{(int)key.CurrentTime.TotalSeconds}.{key.CurrentTime.Milliseconds / 100}";
                }
                var color = key.Palette.GetRelativeColor(GetColorOffset(key.Count, (int)key.CurrentTime.TotalMilliseconds));
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

    public class Key : AViewModel
    {
        public string Name { get; set; }
        [AlsoNotifyFor(nameof(IsActive))] public bool IsActive { get; set; }
        [AlsoNotifyFor(nameof(Brush))] public Brush Brush { get; set; } = Brushes.White;
        [AlsoNotifyFor(nameof(Count))] public int Count { get; set; }
        [AlsoNotifyFor(nameof(Time))] public string Time { get; set; } = "0.0";
        private TimeSpan RawTime;
        public TimeSpan CurrentTime => RawTime + (DateTime.Now - ActivateTime);

        public GradientStopCollection Palette { get; private set; } = new GradientStopCollection(new List<GradientStop>()
        {
            new GradientStop(Colors.White, 0),
            new GradientStop(Colors.Lime, 0.25),
            new GradientStop(Colors.RoyalBlue, 0.50),
            new GradientStop(Colors.Orchid, 0.75),
            new GradientStop(Colors.Orange, 1.0),
        });

        public Key(string Name)
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

    public static class GradientStopCollectionExtensions
    {
        public static Color GetRelativeColor(this GradientStopCollection collection, double offset)
        {
            GradientStop[] stops = collection.OrderBy(x => x.Offset).ToArray();
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[stops.Length - 1].Color;
            GradientStop left = stops[0], right = null;
            foreach (GradientStop stop in stops)
            {
                if (stop.Offset >= offset)
                {
                    right = stop;
                    break;
                }
                left = stop;
            }
            Debug.Assert(right != null);
            offset = Math.Round((offset - left.Offset) / (right.Offset - left.Offset), 2);
            byte a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            byte r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            byte g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            byte b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
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

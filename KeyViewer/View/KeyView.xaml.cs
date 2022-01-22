using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using KeyViewer.ViewModel;

namespace KeyViewer.View
{
    public partial class KeyView : Window
    {
        private KeyViewModel ViewModel => (KeyViewModel)this.DataContext;
        private DispatcherTimer timer;
        public KeyView()
        {
            InitializeComponent();

            //this.Left = 300;
            //this.Top = SystemParameters.PrimaryScreenHeight - this.Height;

            var context = DataContext as KeyViewModel;
            context.AddKey = (k) => KeyContainer.Items.Add(k);
            context.RemoveKey = (k) => KeyContainer.Items.Remove(k);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            timer.Tick += Update;
            //timer.Start();

            var settings = new ProgramSettingsView();
            settings.Init(ViewModel);
            settings.Show();
        }
        private void Update(object sender, EventArgs e)
        {
            Topmost = false;
            Topmost = true;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var context = DataContext as KeyViewModel;
            context.Exit();
        }
    }

    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int WS_EX_APPWINDOW = 0x00040000;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        internal const int WS_EX_TOPMOST = 0x00000008;

        const int GWL_EX_STYLE = -20;

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EX_STYLE);
            SetWindowLong(hwnd, GWL_EX_STYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        public static void SetWindowExHidenInAltTab(IntPtr hwnd)
        {
            //Performing some magic to hide the form from Alt+Tab
            SetWindowLong(hwnd, GWL_EX_STYLE, (GetWindowLong(hwnd, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
        }

        private const int RetrySetTopMostDelay = 200;
        private const int RetrySetTopMostMax = 20;

        public static async Task RetrySetTopMost(IntPtr hwnd)
        {
            for (int i = 0; i < RetrySetTopMostMax; i++)
            {
                await Task.Delay(RetrySetTopMostDelay);
                int winStyle = GetWindowLong(hwnd, GWL_EX_STYLE);

                if ((winStyle & WS_EX_TOPMOST) != 0)
                {
                    break;
                }

                App.Current.MainWindow.Topmost = false;
                App.Current.MainWindow.Topmost = true;
            }
        }
    }
}

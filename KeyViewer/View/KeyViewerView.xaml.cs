using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using KeyViewer.ViewModel;

namespace KeyViewer.View
{
    public partial class KeyViewerView : Window
    {
        private KeyViewerViewModel ViewModel => (KeyViewerViewModel)this.DataContext;
        private DispatcherTimer timer;
        public KeyViewerView()
        {
            InitializeComponent();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };
            timer.Tick += Update;
            //timer.Start();

            var settings = new SettingsView();
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
            WindowsServices.SetWindowExHidenInAltTab(hwnd);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ViewModel.Exit();
        }
    }

    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int WS_EX_APPWINDOW = 0x00040000;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        internal const int WS_EX_TOPMOST = 0x00000008;
        private const int WS_EX_NOACTIVATE = 0x08000000;

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
            SetWindowLong(hwnd, GWL_EX_STYLE, (GetWindowLong(hwnd, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW & ~WS_EX_NOACTIVATE);
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

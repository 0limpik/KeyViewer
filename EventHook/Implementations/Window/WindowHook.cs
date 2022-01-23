using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using EventHook.Abstractions;

namespace EventHook.Implementations.Window
{
    public partial class WindowHook : HookWindow
{
        public event EventHandler<WindowParameters> Changed;

        private static WinEventDelegate ForegroundWindowChanged = null;
        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        public WindowHook()
        {
            Changed?.Invoke(this,CreateWindowParemeters(GetForegroundWindow()));

            ForegroundWindowChanged = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, ForegroundWindowChanged, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        private static string GetWindowTitle(IntPtr hwnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(hwnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) //STATIC
        {
            var windowParameters = CreateWindowParemeters(hwnd);

            if (windowParameters.FilePath == @"C:\Windows\Explorer.EXE")
                return;

            Changed?.Invoke(this,windowParameters);
        }

        private WindowParameters CreateWindowParemeters(IntPtr hwnd)
        {
            GetWindowThreadProcessId(hwnd, out uint pid);
            Process p = Process.GetProcessById((int)pid);

            WindowParameters windowParameters = new WindowParameters();

            windowParameters.FilePath = GetProcessPath((int)pid);
            windowParameters.MainWindowTitle = GetWindowTitle(hwnd);

            try
            {
                windowParameters.StartTime = p.StartTime;
                windowParameters.ProcessName = p.ProcessName?.FirstCharToUpper();
                windowParameters.MainWindowTitle = p.MainWindowTitle ?? windowParameters.MainWindowTitle;
                windowParameters.FilePath = p.MainModule.FileName ?? windowParameters.FilePath;
                windowParameters.ModuleName = p.MainModule.ModuleName;
            }
            catch (Exception ex) { }


            return windowParameters;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);


        /// <summary>
        /// https://stackoverflow.com/questions/8431298/process-mainmodule-access-is-denied
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetProcessPath(int pid)
        {
            var processHandle = OpenProcess(0x1000, false, pid);

            if (processHandle == IntPtr.Zero)
            {
                return null;
            }

            const int lengthSb = 4000;

            var sb = new StringBuilder(lengthSb);

            string result = null;

            if (GetModuleFileNameEx(processHandle, IntPtr.Zero, sb, lengthSb) > 0)
            {
                result = sb.ToString();
            }

            CloseHandle(processHandle);

            return result;
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}

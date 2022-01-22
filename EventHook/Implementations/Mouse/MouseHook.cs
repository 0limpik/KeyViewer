using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EventHook.Abstractions;

namespace EventHook.Implementations.Mouse
{
    public partial class MouseHook : Hook, HookEvents<MouseKeys>, HookPointer
    {
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc process;
        private IntPtr hookId = IntPtr.Zero;
        private const int WH_MOUSE_LL = 14;

        public event EventHandler<MouseKeys> Down;
        public event EventHandler<MouseKeys> Up;
        public event EventHandler<MouseKeys> Event;
        public event EventHandler<(int x, int y)> Moved;

        public MouseHook()
        {
            process = HookCallback;
        }

        public void Hook()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule)
            {
                hookId = SetWindowsHookEx(WH_MOUSE_LL, process, GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(hookId);
        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,

            WM_MOUSEMOVE = 0x0200,

            WM_MOUSEWHEEL = 0x020A,

            WM_LBUTTONDBLCLK = 0x0203,

            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,

            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,

            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var message = (MouseMessages)wParam;

                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

                if (message == MouseMessages.WM_MOUSEMOVE)
                {
                    Moved?.Invoke(this, (hookStruct.pt.x, hookStruct.pt.y));
                    return CallNextHookEx(hookId, nCode, wParam, lParam);
                }

                if (message == MouseMessages.WM_LBUTTONDOWN ||
                    message == MouseMessages.WM_RBUTTONDOWN ||
                    message == MouseMessages.WM_MBUTTONDOWN ||
                    message == MouseMessages.WM_XBUTTONDOWN)
                    Down?.Invoke(this, Convert(message, (int)hookStruct.mouseData));

                if (message == MouseMessages.WM_LBUTTONUP ||
                    message == MouseMessages.WM_RBUTTONUP ||
                    message == MouseMessages.WM_MBUTTONUP ||
                    message == MouseMessages.WM_XBUTTONUP)
                    Up?.Invoke(this, Convert(message, (int)hookStruct.mouseData));

                if (message == MouseMessages.WM_MOUSEWHEEL)
                    Event?.Invoke(this, Convert(message, (short)((hookStruct.mouseData >> 16) & 0xffff)));
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private MouseKeys Convert(MouseMessages messages, int delta)
        {
            switch (messages)
            {
                case MouseMessages.WM_LBUTTONDOWN:
                case MouseMessages.WM_LBUTTONUP:
                    return MouseKeys.Left;

                case MouseMessages.WM_MOUSEWHEEL:
                    if (delta > 0)
                        return MouseKeys.MiddleUp;
                    else
                        return MouseKeys.MiddleDown;

                case MouseMessages.WM_RBUTTONDOWN:
                case MouseMessages.WM_RBUTTONUP:
                    return MouseKeys.Right;

                case MouseMessages.WM_MBUTTONDOWN:
                case MouseMessages.WM_MBUTTONUP:
                    return MouseKeys.Middle;

                case MouseMessages.WM_XBUTTONDOWN:
                case MouseMessages.WM_XBUTTONUP:
                    if (delta == 65536)
                        return MouseKeys.XButton1;
                    else
                        return MouseKeys.XButton2;

                default:
                    return MouseKeys.None;
            }
        }
    }
}

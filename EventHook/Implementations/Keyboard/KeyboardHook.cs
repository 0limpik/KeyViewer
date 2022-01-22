using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using EventHook.Abstractions;

namespace EventHook.Implementations.Keyboard
{
    public partial class KeyboardHook : Hook, HookEvents<KeyboardKeys>
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYUP = 0x105;

        public delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public event EventHandler<KeyboardKeys> Down;
        public event EventHandler<KeyboardKeys> Up;
        public event EventHandler<KeyboardKeys> Event;

        private List<KeyboardKeys> DownedKeys = new List<KeyboardKeys>();

        private KeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        public KeyboardHook()
        {
            _proc = HookCallback;
        }

        public void Hook()
        {
            _hookID = SetHook(_proc);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(KeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                var key = (KeyboardKeys)vkCode;

                if (!DownedKeys.Contains(key))
                {
                    DownedKeys.Add(key);
                    Down?.Invoke(this, key);
                }

            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                var key = (KeyboardKeys)vkCode;

                DownedKeys.Remove(key);
                Up?.Invoke(this, ((KeyboardKeys)vkCode));
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}

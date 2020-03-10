using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Azotium
{
    /// <summary>
    /// Fix solution from: https://qiita.com/SUIMA/items/ea9faeda750248d57306
    /// </summary>
    class WindowUtils
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x00080000;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int VK_F4 = 0x73;

        [DllImport("user32.dll")]
        protected static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        protected static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

        private static void SwitchWindowLongFlag(Window wnd, int nIndex, int dwLong, bool turnOn)
        {
            if(wnd == null)
            {
                return;
            }

            var handle = new WindowInteropHelper(wnd).Handle;
            var extendStyle = GetWindowLong(handle, nIndex);
            if (turnOn)
            {
                extendStyle |= dwLong;
            }
            else
            {
                extendStyle &= ~dwLong;
            }
            SetWindowLong(handle, nIndex, extendStyle);
        }

        public static void SetWindowClickThrough(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_EXSTYLE, WS_EX_TRANSPARENT, true);
        }

        public static void SetWindowNotClickThrough(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_EXSTYLE, WS_EX_TRANSPARENT, false);
        }

        public static void DisableSystemContextMenu(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_STYLE, WS_SYSMENU, false);
        }

        public static void EnableSystemContextMenu(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_STYLE, WS_SYSMENU, true);
        }

        public static void DisableAltF4(Window wnd)
        {
            if(wnd == null)
            {
                return;
            }
            var handle = new WindowInteropHelper(wnd).Handle;
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(AltF4Hook);
        }

        public static void EnableAltF4(Window wnd)
        {
            if(wnd == null)
            {
                return;
            }
            var handle = new WindowInteropHelper(wnd).Handle;
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.RemoveHook(AltF4Hook);
        }

        public static void HideAltTabShown(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_EXSTYLE, WS_EX_TOOLWINDOW, true);
        }

        public static void RecoverAltTabShown(Window wnd)
        {
            SwitchWindowLongFlag(wnd, GWL_EXSTYLE, WS_EX_TOOLWINDOW, false);
        }

        private static IntPtr AltF4Hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr IParam, ref bool handled)
        {
            if (msg == WM_SYSKEYDOWN && wParam.ToInt32() == VK_F4)
            {
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}

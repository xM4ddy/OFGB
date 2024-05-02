using System;
using System.Runtime.InteropServices;

namespace OFGB
{
    public partial class NativeMethod
    {
        [LibraryImport("dwmapi")]
        internal static partial int DwmSetWindowAttribute(nint hwnd, int attr, int[] attrValue, int attrSize);

        // from winuser.h
        public const int GWL_STYLE = -16,
                          WS_MAXIMIZEBOX = 0x10000,
                          WS_MINIMIZEBOX = 0x20000;

        [LibraryImport("user32")]
        internal static partial int GetWindowLongPtrW(IntPtr hwnd, int index);

        [LibraryImport("user32")]
        internal static partial int SetWindowLongPtrW(IntPtr hwnd, int index, int value);
    }
}
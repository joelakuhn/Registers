using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace Registers
{
    class KeyboardMonitor
    {
        public delegate int KeyboardHookProc(int code, int wParam, ref RawKeyboardEventArgs lParam);

        public struct RawKeyboardEventArgs
        {
            public int KeyCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int DWExtraInfo;
        }

        private IntPtr windowHooksReference = IntPtr.Zero;

		const int WH_KEYBOARD_LL = 13;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_SYSKEYDOWN = 0x104;
		const int WM_SYSKEYUP = 0x105;

        public KeyEventHandler KeyDown;
        public KeyEventHandler KeyUp;

        private static KeyboardMonitor instance;

        public static KeyboardMonitor Instance
        {
            get { return instance ?? (instance = new KeyboardMonitor()); }
        }

        private KeyboardMonitor()
        {
			var user32Ref = LoadLibrary("User32");
            windowHooksReference = SetWindowsHookEx(WH_KEYBOARD_LL, HookProc, user32Ref, 0);
        }

        ~KeyboardMonitor()
        {
            UnhookWindowsHookEx(windowHooksReference);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int HookProc(int code, int wParam, ref RawKeyboardEventArgs lParam)
        {
            if (code >= 0)
            {
                var key = (Keys)lParam.KeyCode;
                var args = new KeyEventArgs(key);
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                {
                    KeyDown(this, args);
                }
                else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                {
                    KeyUp(this, args);
                }
                if (args.Handled)
                    return 1;
            }
            return CallNextHookEx(windowHooksReference, code, wParam, ref lParam);
        }

		#region DLL imports
		/// <summary>
		/// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
		/// </summary>
		/// <param name="idHook">The id of the event you want to hook</param>
		/// <param name="callback">The callback.</param>
		/// <param name="hInstance">The handle you want to attach the event to, can be null</param>
		/// <param name="threadId">The thread you want to attach the event to, can be null</param>
		/// <returns>a handle to the desired hook</returns>
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

		/// <summary>
		/// Unhooks the windows hook.
		/// </summary>
		/// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
		/// <returns>True if successful, false otherwise</returns>
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		/// <summary>
		/// Calls the next hook.
		/// </summary>
		/// <param name="idHook">The hook id</param>
		/// <param name="nCode">The hook code</param>
		/// <param name="wParam">The wparam.</param>
		/// <param name="lParam">The lparam.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref RawKeyboardEventArgs lParam);

		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the library</param>
		/// <returns>A handle to the library</returns>
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);
        #endregion
    }
}

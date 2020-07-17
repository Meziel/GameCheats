using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public class GlobalMouseHook
	{
		public event EventHandler<EventArgs> MousePressed;

		private IntPtr _windowsHookHandle;
		private IntPtr _user32LibraryHandle;
		private HookProc _hookProc;

		delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("USER32", SetLastError = true)]
		static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

		[DllImport("USER32", SetLastError = true)]
		public static extern bool UnhookWindowsHookEx(IntPtr hHook);

		[DllImport("USER32", SetLastError = true)]
		static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct LowLevelMouseInputEvent
		{
			/// <summary>
			/// A virtual-key code. The code must be a value in the range 1 to 254.
			/// </summary>
			public int VirtualCode;

			/// <summary>
			/// A hardware scan code for the key. 
			/// </summary>
			public int HardwareScanCode;

			/// <summary>
			/// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
			/// </summary>
			public int Flags;

			/// <summary>
			/// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
			/// </summary>
			public int TimeStamp;

			/// <summary>
			/// Additional information associated with the message. 
			/// </summary>
			public IntPtr AdditionalInformation;
		}

		public const int WH_MOUSE_LL = 14;

		public GlobalMouseHook()
		{
			_windowsHookHandle = IntPtr.Zero;
			_user32LibraryHandle = IntPtr.Zero;
			_hookProc = LowLevelMouseProc;

			_user32LibraryHandle = LoadLibrary("User32");
			if (_user32LibraryHandle == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
			}

			_windowsHookHandle = SetWindowsHookEx(WH_MOUSE_LL, _hookProc, _user32LibraryHandle, 0);
			if (_windowsHookHandle == IntPtr.Zero)
			{
				int errorCode = Marshal.GetLastWin32Error();
				throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// because we can unhook only in the same thread, not in garbage collector thread
				if (_windowsHookHandle != IntPtr.Zero)
				{
					if (!UnhookWindowsHookEx(_windowsHookHandle))
					{
						int errorCode = Marshal.GetLastWin32Error();
						throw new Win32Exception(errorCode, $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
					}
					_windowsHookHandle = IntPtr.Zero;

					// ReSharper disable once DelegateSubtraction
					_hookProc -= LowLevelMouseProc;
				}
			}

			if (_user32LibraryHandle != IntPtr.Zero)
			{
				if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
				{
					int errorCode = Marshal.GetLastWin32Error();
					throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
				}
				_user32LibraryHandle = IntPtr.Zero;
			}
		}

		~GlobalMouseHook()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			MousePressed?.Invoke(this, null);

			return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
		}
	}
}

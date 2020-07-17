using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public static class WindowUtils
	{
		public static IntPtr GetWindowHandle(string windowName, bool exactMatch = false)
		{
			string regString;
			if(exactMatch)
			{
				regString = "^" + windowName + "$";
			}
			else
			{
				regString = windowName;
			}

			return GetWindowHandle(new Regex(regString));
		}

		public static IntPtr GetWindowHandle(Regex windowNameReg)
		{
			IntPtr hWnd = IntPtr.Zero;
			foreach (Process process in Process.GetProcesses())
			{
				if (windowNameReg.IsMatch(process.MainWindowTitle))
				{
					hWnd = process.MainWindowHandle;
					break;
				}
			}

			return hWnd;
		}
	}
}

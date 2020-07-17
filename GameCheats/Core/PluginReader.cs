using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public static class PluginReader
	{
		public static bool Inherits(IPlugin plugin, Type interfaceType)
		{
			if(plugin.GetType().GetInterface(nameof(interfaceType)) != null)
			{
				return true;
			}

			return false;
		}
	}
}

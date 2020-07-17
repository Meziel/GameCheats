using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public static class PluginLoader
	{
		public static IPlugin GetPlugin(string pluginDirectory)
		{
			IPlugin plugin = null;

			AssemblyName assemblyName = AssemblyName.GetAssemblyName(pluginDirectory);
			Assembly assembly = Assembly.Load(assemblyName);

			if (assembly != null)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (type.IsInterface || type.IsAbstract)
					{
						continue;
					}
					else
					{
						if (type.GetInterface(typeof(IPlugin).FullName) != null)
						{
							plugin = (IPlugin)Activator.CreateInstance(type);
						}
					}
				}
			}

			return plugin;
		}
	}
}

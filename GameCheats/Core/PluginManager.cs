using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	class PluginManager : IDisposable
	{
		private GlobalKeyboardHook globalKeyboardHook;
		private GlobalMouseHook globalMouseHook;

		private List<IKeyboardListener> keyboardListeners;
		private List<IMouseListener> mouseListeners;

		public PluginManager()
		{
			globalKeyboardHook = new GlobalKeyboardHook();
			globalMouseHook = new GlobalMouseHook();
			keyboardListeners = new List<IKeyboardListener>();
			mouseListeners = new List<IMouseListener>();
		}

		public void Dispose()
		{
			globalKeyboardHook.Dispose();
			globalMouseHook.Dispose();
		}

		public void Init(List<string> pluginLocations)
		{
			globalKeyboardHook.KeyboardPressed += GlobalKeyboardHook;
			globalMouseHook.MousePressed += GlobalMouseHook;

			foreach (string pluginLocation in pluginLocations)
			{
				IPlugin plugin = PluginLoader.GetPlugin(pluginLocation);
				plugin.Init();

				IKeyboardListener keyboardListener = plugin as IKeyboardListener;
				if(keyboardListener != null)
				{
					keyboardListeners.Add(keyboardListener);
				}

				IMouseListener mouseListener = plugin as IMouseListener;
				if (mouseListener != null)
				{
					mouseListeners.Add(mouseListener);
				}
			}
		}

		private void GlobalKeyboardHook(object sender, GlobalKeyboardHookEventArgs globalKeyboardHookEventArgs)
		{
			foreach (IKeyboardListener keyboardListener in keyboardListeners)
			{
				keyboardListener.OnKeyboardInput(globalKeyboardHookEventArgs.KeyboardData.VirtualCode);
			}
		}

		private void GlobalMouseHook(object sender, EventArgs eventArgs)
		{
			foreach (IMouseListener mouseListener in mouseListeners)
			{
				mouseListener.OnMouseInput(0);
			}
		}
	}
}

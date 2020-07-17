using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameCheats.Core;

namespace TestPlugin
{
	public class TestPlugin : IPlugin, IKeyboardListener, IMouseListener
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public TestPlugin()
		{
			Name = "Test Plugin";
			Description = "A plugin to test the capabilities of the GameCheats framework";
		}

		public void Init()
		{
			
		}

		public void OnKeyboardInput(int virtualCode)
		{
			Console.WriteLine("Virtual Code: {0}", virtualCode);
		}

		public void OnMouseInput(int virtualCode)
		{
			Console.WriteLine("Mouse Code: {0}", virtualCode);
		}
	}
}

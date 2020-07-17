using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public interface IKeyboardListener
	{
		void OnKeyboardInput(int virtualCode);
	}
}

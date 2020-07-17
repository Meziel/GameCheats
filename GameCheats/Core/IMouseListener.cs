using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Core
{
	public interface IMouseListener
	{
		void OnMouseInput(int virtualCode);
	}
}

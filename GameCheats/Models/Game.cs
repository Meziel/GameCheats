using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Models
{
	public class Game
	{
		public int Id { get; set; }
		public string Name { get; set; }

		// Inverse navigation properties
		public ICollection<Configuration> Configurations { get; set; }
	}
}

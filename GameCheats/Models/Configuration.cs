using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Models
{
	public class Configuration
	{
		public int Id { get; set; }
		public string Name { get; set; }

		// Private keys
		public int GameId { get; set; }

		// Navigation properties
		public Game Game { get; set; }
		
		// Inverse properties
		public ICollection<ConfigurationPlugin> ConfigurationsPlugins { get; set; }
	}
}

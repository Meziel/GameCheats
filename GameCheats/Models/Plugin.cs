using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Models
{
	public class Plugin
	{
		public int Id { get; set; }

		public string Location { get; set; }

		// Inverse properties
		public ICollection<ConfigurationPlugin> ConfigurationsPlugins { get; set; }
	}
}

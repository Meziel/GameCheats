using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCheats.Models
{
	[Table("ConfigurationsPlugins")]
	public class ConfigurationPlugin
	{
		public int Id { get; set; }

		// Private keys
		public int ConfigurationId { get; set; }
		public int PluginId { get; set; }

		// Navigation properties
		public Configuration Configuration { get; set; }
		public Plugin Plugin { get; set; }
	}
}

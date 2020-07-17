using GameCheats.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameCheats.Core;

namespace GameCheats.UserControls
{
	/// <summary>
	/// Interaction logic for PluginConfigurationControl.xaml
	/// </summary>
	public partial class PluginConfigurationControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private PluginManager pluginManager = new PluginManager();

		protected void RaisePropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public static readonly DependencyProperty ConfigurationProperty =
			DependencyProperty.Register(
				"Configuration",
				typeof(Configuration),
				typeof(PluginConfigurationControl),
				new PropertyMetadata(ConfigurationChanged)
			);

		public static void ConfigurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PluginConfigurationControl pluginConfigurationControl = (PluginConfigurationControl)sender;
			if (e.NewValue == null)
			{
				pluginConfigurationControl.Plugins = null;
			}
			else
			{
				using (GCDbContext context = new GCDbContext())
				{
					List<Plugin> plugins = context.ConfigurationsPlugins
						.Where(cp => cp.ConfigurationId == ((Configuration)e.NewValue).Id)
						.Select(cp => cp.Plugin)
						.ToList();

					List<PluginInfo> pluginInfos = new List<PluginInfo>();

					foreach (Plugin plugin in plugins)
					{
						IPlugin iplugin = PluginLoader.GetPlugin(plugin.Location);
						
						// Load plugin information
						pluginInfos.Add(new PluginInfo { Name = iplugin.Name, Description = iplugin.Description, Location = plugin.Location });
					}

					pluginConfigurationControl.Plugins = new ObservableCollection<PluginInfo>(pluginInfos);
				}
			}
		}

		public Configuration Configuration
		{
			get
			{
				return (Configuration)GetValue(ConfigurationProperty);
			}
			set
			{
				SetValue(ConfigurationProperty, value);
			}
		}

		private ObservableCollection<PluginInfo> plugins;
		public ObservableCollection<PluginInfo> Plugins
		{
			get
			{
				return plugins;
			}
			set
			{
				plugins = value;
				RaisePropertyChanged();
			}
		}

		public class PluginInfo
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public string Location { get; set; }
		}

		public PluginConfigurationControl()
		{
			InitializeComponent();
			this.DataContext = this;
		}

		~PluginConfigurationControl()
		{
			pluginManager.Dispose();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "DLL files (*.dll)|*dll|All files (*.*)|*.*";

			if (openFileDialog.ShowDialog() == true)
			{
				string dllFileName = openFileDialog.FileName;
				IPlugin iPlugin = PluginLoader.GetPlugin(dllFileName);

				if(iPlugin != null)
				{
					using (GCDbContext context = new GCDbContext())
					{
						Plugin plugin = new Plugin { Location = dllFileName };
						ConfigurationPlugin configurationPlugin = new ConfigurationPlugin();
						configurationPlugin.Plugin = plugin;
						configurationPlugin.ConfigurationId = Configuration.Id;

						context.ConfigurationsPlugins.Add(configurationPlugin);
						if(context.SaveChanges() > 0)
						{
							Plugins.Add(new PluginInfo { Name = iPlugin.Name, Description = iPlugin.Description, Location = dllFileName });
						}
					}
				}
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			List<string> pluginLocations = new List<string>();
			foreach (PluginInfo pluginInfo in Plugins)
			{
				pluginLocations.Add(pluginInfo.Location);
			}
			PluginManager pluginManager = new PluginManager();
			pluginManager.Init(pluginLocations);
		}
	}
}

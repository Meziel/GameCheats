using GameCheats.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace GameCheats.UserControls
{
	/// <summary>
	/// Interaction logic for ConfigurationControl.xaml
	/// </summary>
	public partial class ConfigurationControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public static readonly DependencyProperty GameProperty =
			DependencyProperty.Register(
				"Game",
				typeof(Game),
				typeof(ConfigurationControl),
				new PropertyMetadata(GameChanged)
			);
		public static readonly DependencyProperty SelectedConfigurationProperty =
			DependencyProperty.Register(
				"SelectedConfiguration",
				typeof(Configuration),
				typeof(ConfigurationControl)
			);

		public static void GameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ConfigurationControl configurationControl = (ConfigurationControl)sender;
			if(e.NewValue == null)
			{
				configurationControl.Configurations = null;
			}
			else
			{
				using (GCDbContext context = new GCDbContext())
				{
					configurationControl.Configurations = new ObservableCollection<Configuration>(
						context.Configurations.Where(configuration => configuration.GameId == ((Game)e.NewValue).Id)
					);
				}
			}
		}

		public Game Game
		{
			get
			{
				return (Game)GetValue(GameProperty);
			}
			set
			{
				SetValue(GameProperty, value);
			}
		}

		public ObservableCollection<Configuration> configurations;
		public ObservableCollection<Configuration> Configurations
		{
			get
			{
				return configurations;
			}
			set
			{
				configurations = value;
				RaisePropertyChanged();
			}
		}

		public Configuration SelectedConfiguration
		{
			get
			{
				return (Configuration)GetValue(SelectedConfigurationProperty);
			}
			set
			{
				SetValue(SelectedConfigurationProperty, value);

			}
		}


		public ConfigurationControl()
		{
			SelectedConfiguration = null;

			InitializeComponent();
			this.DataContext = this;
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			// Can't delete if no configuration selected
			if (SelectedConfiguration != null)
			{
				using (GCDbContext context = new GCDbContext())
				{
					// Remove from database
					context.Configurations.Remove(context.Configurations.Find(SelectedConfiguration.Id));
					if (context.SaveChanges() > 0)
					{
						// Only remove locally if database removal was successful
						Configurations.Remove(SelectedConfiguration);
						SelectedConfiguration = null;
					}
				}
			}
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			// TODO: edit selected configuration
		}

		private void New_Click(object sender, RoutedEventArgs e)
		{
			if (Game != null)
			{
				// Open dialog box
				DialogWindow dialogWindow = new DialogWindow("question", "john doe");
				if (dialogWindow.ShowDialog() == true)
				{
					Configuration configuration = new Configuration { Name = dialogWindow.Answer };
					configuration.GameId = Game.Id;

					using (GCDbContext context = new GCDbContext())
					{
						// Add to database
						context.Configurations.Add(configuration);
						if (context.SaveChanges() > 0)
						{
							// Only add locally if database add was successful
							Configurations.Add(configuration);
							SelectedConfiguration = configuration;
						}
					}
				}
			}
		}

		private void ConfigurationCombo_Selected(object sender, RoutedEventArgs e)
		{
			ComboBox combobox = (ComboBox)sender;
			SelectedConfiguration = combobox.SelectedItem as Configuration;
		}
	}
}

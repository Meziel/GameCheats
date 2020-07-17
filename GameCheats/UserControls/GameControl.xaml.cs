using GameCheats.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace GameCheats.UserControls
{
	/// <summary>
	/// Interaction logic for GameControl.xaml
	/// </summary>
	public partial class GameControl : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public static readonly DependencyProperty SelectedGameProperty =
			DependencyProperty.Register(
				"SelectedGame",
				typeof(Game),
				typeof(GameControl)
			);

		public Game SelectedGame
		{
			get
			{
				return (Game)GetValue(SelectedGameProperty);
			}
			set
			{
				SetValue(SelectedGameProperty, value);
			}
		}

		private ObservableCollection<Game> games;
		public ObservableCollection<Game> Games
		{
			get
			{
				return games;
			}
			set
			{
				games = value;
				RaisePropertyChanged();
			}
		}

		public GameControl()
		{
			SelectedGame = null;

			using (GCDbContext context = new GCDbContext())
			{
				Games = new ObservableCollection<Game>(context.Games);
			}

			InitializeComponent();
			this.DataContext = this;
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			// Can't delete if no game selected
			if(SelectedGame != null)
			{
				// Remove from database
				using (GCDbContext context = new GCDbContext())
				{
					context.Games.Remove(context.Games.Find(SelectedGame.Id));
					if (context.SaveChanges() > 0)
					{
						// Only remove locally if database removal was successful
						Games.Remove(SelectedGame);
						SelectedGame = null;
					}
				}
			}
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			// Can't edit if no game selected
			if(SelectedGame != null)
			{
				DialogWindow dialogWindow = new DialogWindow("question", "john doe");
				if (dialogWindow.ShowDialog() == true)
				{
					// Edit database
					using (GCDbContext context = new GCDbContext())
					{
						Game gameFromDatabase = context.Games.Single(game => game.Id == SelectedGame.Id);
						gameFromDatabase.Name = dialogWindow.Answer;
						if (context.SaveChanges() > 0)
						{
							// Only edit locally if database edit was successful
							Games.Remove(SelectedGame);
							Games.Add(gameFromDatabase);
							SelectedGame = gameFromDatabase;
						}
					}
				}
			}
		}

		private void New_Click(object sender, RoutedEventArgs e)
		{
			// Open dialog box
			DialogWindow dialogWindow = new DialogWindow("question", "john doe");
			if (dialogWindow.ShowDialog() == true)
			{
				Game game = new Game { Name = dialogWindow.Answer };

				// Add to database
				using (GCDbContext context = new GCDbContext())
				{
					context.Games.Add(game);
					if (context.SaveChanges() > 0)
					{
						// Only add locally if database add was successful
						Games.Add(game);
						SelectedGame = game;
					}
				}
			}
		}
	}
}

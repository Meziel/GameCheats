using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace GameCheats
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;
		}
	}
}

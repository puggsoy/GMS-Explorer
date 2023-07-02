using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GMS_Explorer
{
	/// <summary>
	/// Interaction logic for ExceptionWindow.xaml
	/// </summary>
	public partial class ExceptionWindow : Window
	{
		public ExceptionWindow()
		{
			InitializeComponent();
		}

		private void OnExitAppClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			Application.Current.Shutdown();
		}
	}
}

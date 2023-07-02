using GMS_Explorer.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GMS_Explorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Dispatcher.UnhandledException += Dispatcher_UnhandledException;
		}

		private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			Current.Dispatcher.BeginInvoke(new Action(() =>
			{
				ExceptionWindow ew = new ExceptionWindow();
				ew.DataContext = new ExceptionWindowVM(e.Exception);
				ew.Show();
			}));

			e.Handled = true;
		}
	}
}

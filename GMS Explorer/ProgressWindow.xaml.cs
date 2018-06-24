using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GMS_Explorer
{
	/// <summary>
	/// Interaction logic for ProgressWindow.xaml
	/// </summary>
	public partial class ProgressWindow : Window
	{
		public CancellationToken CancelToken { get { return cts.Token; } }
		private CancellationTokenSource cts;
		

		#region Remove Close Button
		private const int GWL_STYLE = -16;
		private const int WS_SYSMENU = 0x80000;

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			var hwnd = new WindowInteropHelper(this).Handle;
			SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
		}
		#endregion

		public ProgressWindow(double max)
		{
			InitializeComponent();

			PBar.Maximum = max;

			cts = new CancellationTokenSource();
		}

		public void UpdateProgress(double amount)
		{
			PBar.Value = amount;
		}

		public void IncrementProgress(double amount)
		{
			PBar.Value += amount;
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			cts.Cancel();
		}
	}
}

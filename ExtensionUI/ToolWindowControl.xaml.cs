namespace ExtensionUI
{
	using ExtensionUI.ViewModels;
	using Microsoft.VisualStudio;
	using Microsoft.VisualStudio.Shell.Interop;
	using System;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Shapes;

	/// <summary>
	/// Interaction logic for ToolWindowControl.
	/// </summary>
	public partial class ToolWindowControl : UserControl
	{
		public ToolViewModel ToolViewModel { get; set; } = new ToolViewModel();
		public StackPanel StackPanel { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
		/// </summary>
		public ToolWindowControl()
		{
			ToolViewModel.ItemList = new System.Collections.ObjectModel.ObservableCollection<string>();
			ToolViewModel.ItemList.Add("Angular");
			ToolViewModel.ItemList.Add("Vue");
			ToolViewModel.ItemList.Add("Maven");

			ToolViewModel.PickedFolderPath = "Select folder";

			this.InitializeComponent();

			this.DataContext = ToolViewModel;

			this.StackPanel = (StackPanel)this.FindName("stckpnl");
		}

		private void btn_browse_Click(object sender, RoutedEventArgs e)
		{
			using( var dialog = new System.Windows.Forms.FolderBrowserDialog() )
			{
				var dialogResult = dialog.ShowDialog();

				if( dialogResult == System.Windows.Forms.DialogResult.OK )
				{
					ToolViewModel.PickedFolderPath = dialog.SelectedPath;
					ToolViewModel.IsEnabled = true;
				}
			}
		}

		private void cmb_commands_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var comboBox = new ComboBox()
			{
				Margin = new Thickness(15, 10, 15, 10),
				Width = 200,
				Height = 20,
				FontSize = 12,
				IsTextSearchEnabled = false,
				Name = "sandor",
				ItemsSource = ToolViewModel.ItemList,
			};

			comboBox.SelectionChanged += cmb_commands_SelectionChanged;

			Grid.SetRow(comboBox, 2);
			Grid.SetColumn(comboBox, 1);

			this.StackPanel.Children.Add(comboBox);
		}

		private async void Btn_run_Click(object sender, RoutedEventArgs e)
		{
			string cmd = "ng.cmd";
			string args = "new proba --defaults --routing";

			// Working directory - deafult is Solution folder
			// TODO do not set it here / rather set this as default where user sets the folder
			var dte = (EnvDTE.DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE));
			string path = System.IO.Path.GetDirectoryName(dte.Solution.FullName);

			await Task.Run(() => ExecuteCmd(cmd, args, path));
		}

		private async Task ExecuteCmd(string cmd, string args, string path)
		{
			// Create Output window
			var outputWindow = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
			var paneGuid = VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
			outputWindow.CreatePane(paneGuid, "CLIHelper", 1, 0);
			outputWindow.GetPane(paneGuid, out IVsOutputWindowPane pane);

			// Command to be executed
			Process pProcess = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\npm\\{cmd}",
					Arguments = args,
					CreateNoWindow = true,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					WorkingDirectory = path
				}
			};
			pProcess.Start();

			// Printig output and error to the created output window
			var strOutput = pProcess.StandardOutput;
			var strError = pProcess.StandardError;
			while( !strOutput.EndOfStream )
			{
				pane.OutputString(strOutput.ReadLine() + "\n");
			}
			while( !strError.EndOfStream )
			{
				pane.OutputString(strError.ReadLine() + "\n");
			}
		}

	}
}
namespace ExtensionUI
{
    using ExtensionUI.ViewModels;
    using Model.Parser;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Model.Models;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Interaction logic for ToolWindowControl.
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        public ToolViewModel ToolViewModel { get; set; } = new ToolViewModel();
        public TextBox ArgumentTextBox { get; set; }
        public ListBox OptionsListBox { get; set; }

        private Command _selectedCommand;
        private Argument _selectedArgument;
        private List<Option> _selectedOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
        /// </summary>
        public ToolWindowControl()
        {
            ToolViewModel.CliTypes = new System.Collections.ObjectModel.ObservableCollection<string>();
            ToolViewModel.CliTypes.Add("commands");

            ToolViewModel.Commands = new System.Collections.ObjectModel.ObservableCollection<Command>();

			ToolViewModel.PickedFolderPath = "Select folder";

			this.InitializeComponent();

			this.DataContext = ToolViewModel;

            this.ArgumentTextBox = (TextBox)this.FindName("txt_argument");
            this.OptionsListBox = (ListBox)this.FindName("lst_options");
        }

		private void btn_browse_Click(object sender, RoutedEventArgs e)
		{
			using( var dialog = new System.Windows.Forms.FolderBrowserDialog() )
			{
				var dialogResult = dialog.ShowDialog();

                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    ToolViewModel.PickedFolderPath = dialog.SelectedPath;

                    ToolViewModel.IsEnabled = true;
                }
            }
        }

        private void cmd_cli_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCliType = e.AddedItems[0]?.ToString();

            if (!String.IsNullOrEmpty(selectedCliType))
            {
                var parser = new CliCommandParser();
                var commandList = parser.Deserialize(selectedCliType);

                foreach (var command in commandList)
                {
                    ToolViewModel.Commands.Add(command);
                }

                ToolViewModel.IsEnabled = true;
            }
        }

        private void cmb_commands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectedCommand = (Command)e.AddedItems[0];

            if (_selectedCommand != null)
            {
                var comboBox = (ComboBox)this.FindName("cmb_arguments");
                if (_selectedCommand.Arguments != null && _selectedCommand.Arguments.Count > 0)
                {
                    comboBox.Visibility = Visibility.Visible;
                    comboBox.ItemsSource = _selectedCommand.Arguments;

                    ArgumentTextBox.Visibility = Visibility.Collapsed;

                    this._selectedArgument = null;
                    this._selectedOptions = null;

                    GenerateFullCommandText();
                }
                else
                {
                    comboBox.Visibility = Visibility.Collapsed;
                }

                var listBox = (ListBox)this.FindName("lst_options");
                if (_selectedCommand.Options != null && _selectedCommand.Options.Count > 0)
                {
                    listBox.Visibility = Visibility.Visible;
                    listBox.ItemsSource = _selectedCommand.Options;
                }
                else
                {
                    listBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cmb_arguments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                this._selectedArgument = (Argument)e.AddedItems[0];

                if (_selectedArgument != null)
                {
                    if (_selectedArgument.NumberOfParams > 1)
                    {
                        ArgumentTextBox.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ArgumentTextBox.Visibility = Visibility.Collapsed;
                    }

                    GenerateFullCommandText();
                }
            }
        }

        private void GenerateFullCommandText()
        {
            string options = "";
            if (OptionsListBox.SelectedItems.Count > 0)
            {
                options = "--" + String.Join(" --", OptionsListBox.SelectedItems.Cast<Option>().Select(o => o.Name).ToArray());
            }

            var commandList = new List<string>
            {
                "ng",
                _selectedCommand?.Name,
                _selectedArgument?.Name,
                ArgumentTextBox?.Text,
                options
            };

            ToolViewModel.FullCommandText = String.Join(" ", commandList.ToArray());
        }

        private void txt_argument_TextChanged(object sender, TextChangedEventArgs e)
        {
            GenerateFullCommandText();
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
namespace ExtensionUI
{
    using ExtensionUI.ViewModels;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;
    using System.Diagnostics;
    using global::Model.Models;
    using global::Model.Parser;
    using System.Collections.ObjectModel;
    using ExtensionUI.Helper;

    /// <summary>
    /// Interaction logic for ToolWindowControl.
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        private ToolViewModel ToolViewModel { get; set; } = new ToolViewModel();
        private Command _selectedCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
        /// </summary>
        public ToolWindowControl()
        {
            ToolViewModel.CliTypes = new ObservableCollection<string>();
            ToolViewModel.CliTypes.Add("angular");

            ToolViewModel.Commands = new ObservableCollection<Command>();

			ToolViewModel.PickedFolderPath = "Select folder";

            ToolViewModel.Arguments = new ItemsChangeObservableCollection<Model.ArgumentModel>();
            ToolViewModel.Options = new ItemsChangeObservableCollection<Model.OptionModel>();

            this.InitializeComponent();

            this.DataContext = ToolViewModel;
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
				var dte = (EnvDTE.DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE));
				string path = System.IO.Path.GetDirectoryName(dte.Solution.FullName);

				dialog.SelectedPath = path;
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
            }
        }

        private void cmb_commands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectedCommand = (Command)e.AddedItems[0];

            if (_selectedCommand != null)
            {
                ToolViewModel.Arguments.Clear();
                var argumentExpander = (Expander)this.FindName("exp_arguments");

                if (_selectedCommand.Arguments != null && _selectedCommand.Arguments.Count > 0)
                {
                    foreach (var argument in _selectedCommand.Arguments)
                    {
                        var argumentModel = new Model.ArgumentModel()
                        {
                            Name = argument.Name,
                            Alias = argument.Alias,
                            Description = argument.Description,
                            NumberOfParams = argument.NumberOfParams,
                            IsSelected = false,
                        };

                        ToolViewModel.Arguments.Add(argumentModel);
                    }

                    argumentExpander.IsEnabled = true;
                }
                else if (_selectedCommand.Arguments != null && _selectedCommand.Arguments.Count == 0)
                {
                    argumentExpander.IsEnabled = false;
                    argumentExpander.IsExpanded = false;
                }

                ToolViewModel.Options.Clear();
                var optionsExpander = (Expander)this.FindName("exp_options");

                if (_selectedCommand.Options != null && _selectedCommand.Options.Count > 0)
                {
                    foreach (var option in _selectedCommand.Options)
                    {
                        var optionModel = new Model.OptionModel()
                        {
                            Name = option.Name,
                            Alias = option.Alias,
                            Description = option.Description,
                            OptionType = option.OptionType,
                            IsChecked = false,
                        };

                        ToolViewModel.Options.Add(optionModel);
                    }

                    optionsExpander.IsEnabled = true;
                }
                else if (_selectedCommand.Options != null && _selectedCommand.Options.Count == 0)
                {
                    optionsExpander.IsEnabled = false;
                    optionsExpander.IsExpanded = false;
                }
            }
        }

        private string GenerateFullCommandText()
        {
			string cmd = "ng";
			string args = GenerateCommand();
			return $"{cmd} {args}";
		}

		private async void Btn_run_Click(object sender, RoutedEventArgs e)
        {
            var invalidLabel = (Label)this.FindName("lbl_invalid");

            if (ToolViewModel.InvalidCommand)
            {
                invalidLabel.Visibility = Visibility.Visible;
            }
            else
            {
                invalidLabel.Visibility = Visibility.Hidden;

                string cmd = "ng.cmd";
				string args = GenerateCommand();
                //string args = "new proba --defaults --routing";
				var path = ToolViewModel.PickedFolderPath;

                await Task.Run(() => ExecuteCmd(cmd, args, path));
            }
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
            while (!strOutput.EndOfStream)
            {
                pane.OutputString(strOutput.ReadLine() + "\n");
            }
            while (!strError.EndOfStream)
            {
                pane.OutputString(strError.ReadLine() + "\n");
            }
        }

        private void btn_copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ToolViewModel.FullCommandText);
        }

        private void btn_preview_Click(object sender, RoutedEventArgs e)
        {
            ToolViewModel.FullCommandText = GenerateFullCommandText();
        }

        private string GenerateCommand()
        {
            ToolViewModel.InvalidCommand = false;

            //var stringBuilder = new StringBuilder("ng");
            var stringBuilder = new StringBuilder();

            if (_selectedCommand != null)
                stringBuilder.Append(" " + _selectedCommand.Name);

            foreach (var argument in ToolViewModel.Arguments)
            {
                if (argument.IsSelected)
                {
                    if (argument.NumberOfParams > 0)
                    {
                        if (!String.IsNullOrEmpty(argument.ArgumentValue) && (argument.ArgumentValue.Split(' ').Length == argument.NumberOfParams))
                            stringBuilder.Append(" " /*+ argument.Name + "="*/ + argument.ArgumentValue);
                        else
                            ToolViewModel.InvalidCommand = true;
                    }
                    else
                    {
                        stringBuilder.Append(" " + argument.Name);
                    }
                }
            }

            foreach (var option in ToolViewModel.Options)
            {
                if (option.IsChecked)
                {
                    stringBuilder.Append(" --" + option.Name);
                }
                else if (!String.IsNullOrEmpty(option.OptionValue))
                {
                    stringBuilder.Append(" --" + option.Name + "=" + option.OptionValue);
                }
            }

            return stringBuilder.ToString();
        }

        private void btn_upload_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var streamReader = new System.IO.StreamReader(fileDialog.FileName);
                System.IO.File.WriteAllText(Environment.CurrentDirectory,streamReader.ReadToEnd());
                streamReader.Close();
            }
        }
    }
}
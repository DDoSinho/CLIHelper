namespace ExtensionUI
{
    using ExtensionUI.ViewModels;
    using System;
    using System.Diagnostics.CodeAnalysis;
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
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();

                if (dialogResult == System.Windows.Forms.DialogResult.OK)
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
    }
}
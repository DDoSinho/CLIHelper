namespace ExtensionUI
{
    using ExtensionUI.ViewModels;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ToolWindowControl.
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        public ToolViewModel ToolViewModel { get; set; } = new ToolViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
        /// </summary>
        public ToolWindowControl()
        {
            ToolViewModel.ItemList = new System.Collections.ObjectModel.ObservableCollection<string>();
            ToolViewModel.ItemList.Add("alma1");
            ToolViewModel.ItemList.Add("alma2");
            ToolViewModel.ItemList.Add("alma3");
            ToolViewModel.ItemList.Add("almafa");
            ToolViewModel.ItemList.Add("barack1");
            ToolViewModel.ItemList.Add("barack2");
            ToolViewModel.ItemList.Add("barack3");
            ToolViewModel.ItemList.Add("barack4");
            ToolViewModel.ItemList.Add("barackfa");

            this.InitializeComponent();

            ToolViewModel.PickedFolderPath = "Select folder";
            this.DataContext = ToolViewModel;
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();

                if(dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    ToolViewModel.PickedFolderPath = dialog.SelectedPath;
                }
            }
        }
    }
}
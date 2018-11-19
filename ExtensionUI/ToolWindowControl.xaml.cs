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
        public FileViewModel FileViewModel { get; set; } = new FileViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
        /// </summary>
        public ToolWindowControl()
        {
            this.InitializeComponent();
            this.DataContext = FileViewModel;
        }

        private void btn_browse_Click(object sender, RoutedEventArgs e)
        {
            using(var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();

                if(dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    FileViewModel.PickedFolderPath = dialog.SelectedPath;
                }
            }
        }
    }
}
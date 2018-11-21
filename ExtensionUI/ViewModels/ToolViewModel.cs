using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionUI.ViewModels
{
    public class ToolViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _pickedFolderPath;
        public string PickedFolderPath
        {
            get =>  _pickedFolderPath;
            set
            {
                _pickedFolderPath = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PickedFolderPath)));
            }
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value)
                    return;

                _searchText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchText)));
            }
        }

        public ObservableCollection<string> ItemList { get; set; }
    }
}

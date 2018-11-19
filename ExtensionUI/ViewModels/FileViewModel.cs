using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionUI.ViewModels
{
    public class FileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _pickedFolderPath;
        public string PickedFolderPath
        {
            get
            {
                return _pickedFolderPath;
            }
            set
            {
                _pickedFolderPath = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PickedFolderPath)));
            }
        }


    }
}

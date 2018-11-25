using ExtensionUI.Helper;
using ExtensionUI.Model;
using Model.Models;
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
        public ObservableCollection<string> CliTypes { get; set; }
        public ObservableCollection<Command> Commands { get; set; }

        private string _pickedFolderPath;
        public string PickedFolderPath
        {
            get => _pickedFolderPath;
            set
            {
                if (_pickedFolderPath == value)
                    return;

                _pickedFolderPath = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PickedFolderPath)));
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
            }
        }

        private string _fullCommandText;
        public string FullCommandText
        {
            get => _fullCommandText;
            set
            {
                if (_fullCommandText == value)
                    return;

                _fullCommandText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullCommandText)));
            }
        }

        public ItemsChangeObservableCollection<OptionModel> Options { get; set; }
        public ItemsChangeObservableCollection<ArgumentModel> Arguments { get; set; }

    }
}

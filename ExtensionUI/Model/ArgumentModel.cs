using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionUI.Model
{
    public class ArgumentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public int NumberOfParams { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
            }
        }

        private string _argumentValue;
        public string ArgumentValue
        {
            get => _argumentValue;
            set
            {
                if (_argumentValue == value)
                    return;

                _argumentValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArgumentValue)));
            }
        }

        public bool IsEditable
        {
            get => NumberOfParams != 0 && IsSelected;
        }

        public string ArgumentToolTip
        {
            get => $"{NumberOfParams} parameter(s) should add (seperate with withspace)";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionUI.Model
{
    public class OptionModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

        public string InputType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                    return;

                _isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }

        private string _optionValue;
        public string OptionValue
        {
            get => _optionValue;
            set
            {
                if (_optionValue == value)
                    return;

                _optionValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionValue)));
            }
        }
    }
}

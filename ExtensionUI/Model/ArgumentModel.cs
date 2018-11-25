using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionUI.Model
{
    public class ArgumentModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public int NumberOfParams { get; set; }
        public bool IsSelected { get; set; }
        public string ArgumentValue { get; set; }

        public bool IsEditable
        {
            get => NumberOfParams != 0;
        }

        public string ArgumentToolTip
        {
            get => $"{NumberOfParams} parameter(s) should add (seperate with withspace)";
        }

    }
}

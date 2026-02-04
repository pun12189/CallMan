using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class DynamicField : ObservableObject
    {
        private string label;
        private string controlType;
        private List<string> options;
        private object _value;

        public string Label { get => label; set => Set(ref label, value, nameof(Label)); }
        public string ControlType { get => controlType; set => Set(ref controlType, value, nameof(ControlType)); } // "Text", "Dropdown", "Check"
        public List<string> Options { get => options; set => Set(ref options, value, nameof(Options)); } // Only for Dropdown
        public object Value { get => _value; set => Set(ref _value, value, nameof(Value)); } // Stores the user's input
    }
}

using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models.Common
{
    public class ColumnItemModel : ObservableObject
    {
        private bool _isVisible = true;
        private string header = string.Empty;

        public string Header { get => header; set => Set(ref header, value, nameof(Header)); }       

        public bool IsVisible { get => _isVisible; set => Set(ref _isVisible, value, nameof(IsVisible)); }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class MappingItem
    {
        public string DbColumnName { get; set; }     // e.g., "CustomerName"
        public string SelectedExcelHeader { get; set; } // e.g., "Client Name" (from Excel)
    }
}

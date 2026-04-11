using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class MappingRule
    {
        public string DbColumnName { get; set; } // Actual MySQL column name
        public string ExcelHeader { get; set; }  // Selected Excel header
    }
}

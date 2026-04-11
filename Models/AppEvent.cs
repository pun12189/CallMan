using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class AppEvent
    {
        // This matches the 'EventKey' in your MySQL table (e.g., "OnLeadCreated")
        public string EventKey { get; set; }

        // This holds the actual data object (The Lead, Invoice, or Task)
        public object TargetData { get; set; }
    }
}

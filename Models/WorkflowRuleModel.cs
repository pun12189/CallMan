using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class WorkflowRuleModel
    {
        public int Id { get; set; }
        public string EventKey { get; set; }
        public bool IsActive { get; set; } = true;
        public bool SendWhatsApp { get; set; }
        public bool SendEmail { get; set; }
        public string MessageTemplate { get; set; }
    }
}

using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class WorkflowRuleModel : ObservableObject
    {
        public int Id { get; set; }
        public string EventKey { get; set; }
        public bool IsActive { get; set; } = true;
        public bool SendWhatsApp { get; set; }
        public bool SendEmail { get; set; }
        public string MessageTemplate { get; set; }

        private int _inactivityDays = 30; // Default
        public int InactivityDays
        {
            get => _inactivityDays;
            set => Set(ref _inactivityDays, value, nameof(InactivityDays));
        }
    }
}

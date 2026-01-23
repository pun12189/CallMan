using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class LeadFollowUpModel : ObservableObject
    {
        private string _name;
        [Required]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        private string _lastMsg;
        public string LastMsg
        {
            get => _lastMsg;
            set => Set(ref _lastMsg, value, nameof(LastMsg));
        }

        private DateTime _nextFollowup;
        public DateTime NextFollowUp
        {
            get => _nextFollowup;
            set => Set(ref _nextFollowup, value, nameof(NextFollowUp));
        }

        private int _leadId;
        public int LeadId
        {
            get => _leadId;
            set => Set(ref _leadId, value, nameof(LeadId));
        }

        private DateTime _createFollowup;
        public DateTime CreateFollowUp
        {
            get => _createFollowup;
            set => Set(ref _createFollowup, value, nameof(CreateFollowUp));
        }
    }
}

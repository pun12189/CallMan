using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class LeadDeadModel : ObservableObject
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

        private int _leadId;
        public int LeadId
        {
            get => _leadId;
            set => Set(ref _leadId, value, nameof(LeadId));
        }

        private DateTime _createDead;
        public DateTime CreateDead
        {
            get => _createDead;
            set => Set(ref _createDead, value, nameof(CreateDead));
        }
    }
}

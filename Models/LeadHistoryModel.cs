using BahiKitab.Core;
using BahiKitab.Helper;
using System.ComponentModel.DataAnnotations;

namespace BahiKitab.Models
{
    public class LeadHistoryModel : ObservableObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => Set(ref _id, value, nameof(Id));
        }

        private string _name;
        [Required]
        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate
        {
            get => _creationDate;
            set => Set(ref _creationDate, value, nameof(CreationDate));
        }

        private LeadType _leadType;
        public LeadType LeadType
        {
            get => _leadType;
            set => Set(ref _leadType, value, nameof(LeadType));
        }

        private int _leadid;
        public int LeadId
        {
            get => _leadid;
            set => Set(ref _leadid, value, nameof(LeadId));
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
    }
}

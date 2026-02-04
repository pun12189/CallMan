using BahiKitab.Core;
using BahiKitab.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    // Inherit from ObservableObject so the View can react to property changes
    public class Lead : ObservableObject, ICloneable
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

        private string _company;
        public string Company
        {
            get => _company;
            set => Set(ref _company, value, nameof(Company));
        }

        private string _email;
        [EmailAddress]
        public string Email
        {
            get => _email;
            set => Set(ref _email, value, nameof(Email));
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value, nameof(Phone));
        }

        private string _stage; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string Stage
        {
            get => _stage;
            set => Set(ref _stage, value, nameof(Stage));
        }

        private LeadStatusModel _status; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public LeadStatusModel Status
        {
            get => _status;
            set => Set(ref _status, value, nameof(Status));
        }

        private LeadSourceModel _leadSource; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public LeadSourceModel LeadSource
        {
            get => _leadSource;
            set => Set(ref _leadSource, value, nameof(LeadSource));
        }

        private LeadTagModel _tags; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public LeadTagModel Tags
        {
            get => _tags;
            set => Set(ref _tags, value, nameof(Tags));
        }

        private LeadLabelsModel _label; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public LeadLabelsModel Label
        {
            get => _label;
            set => Set(ref _label, value, nameof(Label));
        }

        private string _country; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string Country
        {
            get => _country;
            set => Set(ref _country, value, nameof(Country));
        }

        private string _state; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string State
        {
            get => _state;
            set => Set(ref _state, value, nameof(State));
        }

        private string _city; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string City
        {
            get => _city;
            set => Set(ref _city, value, nameof(City));
        }

        private string _district; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string District
        {
            get => _district;
            set => Set(ref _district, value, nameof(District));
        }

        private string _pincode; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string Pincode
        {
            get => _pincode;
            set => Set(ref _pincode, value, nameof(Pincode));
        }

        private string _firmname; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string FirmName
        {
            get => _firmname;
            set => Set(ref _firmname, value, nameof(FirmName));
        }

        private DateTime _impdate; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public DateTime ImpDate
        {
            get => _impdate;
            set => Set(ref _impdate, value, nameof(ImpDate));
        }

        private string _altphone; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string AltPhone
        {
            get => _altphone;
            set => Set(ref _altphone, value, nameof(AltPhone));
        }        

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate
        {
            get => _creationDate;
            set => Set(ref _creationDate, value, nameof(CreationDate));
        }

        private DateTime _updationDate = DateTime.Now;
        public DateTime UpdationDate
        {
            get => _updationDate;
            set => Set(ref _updationDate, value, nameof(UpdationDate));
        }

        private LeadType _leadType = LeadType.New;
        public LeadType LeadType
        {
            get => _leadType;
            set => Set(ref _leadType, value, nameof(LeadType));
        }

        private LeadFollowUpModel _leadFollowUpModel;
        public LeadFollowUpModel LeadFollowUpModel
        {
            get => _leadFollowUpModel;
            set => Set(ref _leadFollowUpModel, value, nameof(LeadFollowUpModel));
        }

        private LeadDeadModel _leadDeadModel;
        public LeadDeadModel LeadDeadModel
        {
            get => _leadDeadModel;
            set => Set(ref _leadDeadModel, value, nameof(LeadDeadModel));
        }

        private StaffModel _leadHolder;
        public StaffModel LeadHolder
        {
            get => _leadHolder;
            set => Set(ref _leadHolder, value, nameof(LeadHolder));
        }

        public Lead Clone() { return (Lead)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

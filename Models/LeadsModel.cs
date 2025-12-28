using BahiKitab.Core;
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

        private string _leadSource; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string LeadSource
        {
            get => _leadSource;
            set => Set(ref _leadSource, value, nameof(LeadSource));
        }

        private string _tags; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string Tags
        {
            get => _tags;
            set => Set(ref _tags, value, nameof(Tags));
        }

        private string _label; // Corresponds to Lead Stages (e.g., New, Qualified, Contacted)
        public string Label
        {
            get => _label;
            set => Set(ref _label, value, nameof(Label));
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

        public Lead Clone() { return (Lead)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

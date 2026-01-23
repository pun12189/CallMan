using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class BrandModel : ObservableObject
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

        private int _inventoryId;
        public int InventoryId
        {
            get => _inventoryId;
            set => Set(ref _inventoryId, value, nameof(InventoryId));
        }

        private int _customerId;
        public int CustomerId
        {
            get => _customerId;
            set => Set(ref _customerId, value, nameof(CustomerId));
        }
    }
}

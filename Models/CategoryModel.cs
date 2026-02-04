using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class CategoryModel : ObservableObject
    {
        private int id;
        private string name;
        private ObservableCollection<CategoryModel> subCategories = new();
        private int? parentId;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }

        public int? ParentId { get => parentId; set => Set(ref parentId, value, nameof(ParentId)); } // Null for Root Categories

        public required string Name { get => name; set => Set(ref name, value, nameof(Name)); }

        public ObservableCollection<CategoryModel> SubCategories { get => subCategories; set => Set(ref subCategories, value, nameof(SubCategories)); }
    }
}

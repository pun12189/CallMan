using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class TaskModel : ObservableObject, ICloneable
    {
        private string orderId;
        private Lead customer;
        private ObservableCollection<StaffModel> staff = new();
        private ObservableCollection<DepartmentsModel> departments = new();
        private ProductModel product;
        private bool isAccepted;
        private string remarks;
        private int id;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }

        public string OrderId { get => orderId; set => Set(ref orderId, value, nameof(OrderId)); }

        public Lead Customer { get => customer; set => Set(ref customer, value, nameof(Customer)); }

        public ObservableCollection<StaffModel> Staff { get => staff; set => Set(ref staff, value, nameof(Staff)); }

        public ObservableCollection<DepartmentsModel> Departments { get => departments; set => Set(ref departments, value, nameof(Departments)); }

        public ProductModel Product { get => product; set => Set(ref product, value, nameof(Product)); }

        public bool IsAccepted { get => isAccepted; set => Set(ref isAccepted, value, nameof(IsAccepted)); }

        public string Remarks { get => remarks; set => Set(ref remarks, value, nameof(Remarks)); }

        public TaskModel Clone() { return (TaskModel)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

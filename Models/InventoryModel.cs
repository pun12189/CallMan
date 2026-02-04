using BahiKitab.Core;
using BahiKitab.Helper;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class InventoryModel : ObservableObject, ICloneable
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

        private string _shortCode;
        public string ShortCode
        {
            get => _shortCode;
            set => Set(ref _shortCode, value, nameof(ShortCode));
        }

        private CategoryModel _category;
        public CategoryModel Category
        {
            get => _category;
            set => Set(ref _category, value, nameof(Category));
        }

        private double _stock;
        public double Stock
        {
            get => _stock;
            set => Set(ref _stock, value, nameof(Stock));
        }

        private double _sku;
        public double SKU
        {
            get => _sku;
            set => Set(ref _sku, value, nameof(SKU));
        }

        private double _totalOrders;
        public double TotalOrders
        {
            get => _totalOrders;
            set => Set(ref _totalOrders, value, nameof(TotalOrders));
        }

        private double _remainingStock;
        public double RemainingStock
        {
            get => _remainingStock;
            set => Set(ref _remainingStock, value, nameof(RemainingStock));
        }

        private double _totalProductCount;
        public double TotalProductCount
        {
            get => _totalProductCount;
            set => Set(ref _totalProductCount, value, nameof(TotalProductCount));
        }

        private double _totalBrandCount;
        public double TotalBrandCount
        {
            get => _totalBrandCount;
            set => Set(ref _totalBrandCount, value, nameof(TotalBrandCount));
        }

        private double _totalRequired;
        public double TotalRequired
        {
            get => _totalRequired;
            set
            {
                Set(ref _totalRequired, value, nameof(TotalRequired));
                this.RemainingStock = this.Stock - value;
            }
        }

        private ProductUnits units;
        public ProductUnits Units
        {
            get => units;
            set => Set(ref units, value, nameof(Units));
        }

        private double _gst;
        public double GST
        {
            get => _gst;
            set => Set(ref _gst, value, nameof(GST));
        }

        private double _purchasePrice;
        public double PurchasePrice
        {
            get => _purchasePrice;
            set => Set(ref _purchasePrice, value, nameof(PurchasePrice));
        }

        private double _sellingPrice;
        public double SellingPrice
        {
            get => _sellingPrice;
            set => Set(ref _sellingPrice, value, nameof(SellingPrice));
        }

        private DateTime created;        
        public DateTime Created { get => created; set => Set(ref created, value, nameof(Created)); }

        private DateTime updated;
        public DateTime Updated { get => updated; set => Set(ref updated, value, nameof(Updated)); }

        public InventoryModel Clone() { return (InventoryModel)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

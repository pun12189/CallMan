using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class PurchaseItem : ObservableObject
    {
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public string HSN { get; set; }

        private int _qty;
        public int Qty
        {
            get => _qty;
            set { Set(ref _qty, value, nameof(Qty)); RefreshRow(); }
        }

        private double _pricePerItem;
        public double PricePerItem
        {
            get => _pricePerItem;
            set { Set(ref _pricePerItem, value, nameof(PricePerItem)); RefreshRow(); }
        }

        private double _taxPercent;
        public double TaxPercent
        {
            get => _taxPercent;
            set { Set(ref _taxPercent, value, nameof(TaxPercent)); RefreshRow(); }
        }

        // --- The Missing Calculations ---

        // Amount before tax: Qty * Price
        public double TaxableAmount => Qty * PricePerItem;

        // The GST amount for this row: (Taxable * Rate) / 100
        public double TaxValue => TaxableAmount * (TaxPercent / 100);

        // Total for this row: Taxable + Tax
        public double TotalLine => TaxableAmount + TaxValue;

        private void RefreshRow()
        {
            // Tell the UI that the calculated fields have changed
            OnPropertyChanged(nameof(TaxableAmount));
            OnPropertyChanged(nameof(TaxValue));
            OnPropertyChanged(nameof(TotalLine));
        }
    }
}

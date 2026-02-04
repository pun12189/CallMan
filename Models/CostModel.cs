using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class CostModel : ObservableObject, ICloneable
    {
        private double quantity;
        private double rate;
        private double discount;
        private double gST;
        private double totalPrice;

        public double Quantity { get => quantity; 
            set 
            {
                Set(ref quantity, value, nameof(Quantity));
                var tp = (this.Quantity * this.Rate);
                if (this.GST <= 0.0)
                {
                    this.TotalPrice = tp;
                }
                else
                {
                    this.TotalPrice = tp + ((this.GST / 100) * tp);
                }                
            }
        }

        public double Rate { get => rate;
            set 
            {
                Set(ref rate, value, nameof(Rate));
                /*var tp = (this.Quantity * this.Rate);
                if (this.GST <= 0.0)
                {
                    this.TotalPrice = tp;
                }
                else
                {
                    this.TotalPrice = tp + ((this.GST / 100) * tp);
                }*/
            }
        }

        public double Discount { get => discount; 
            set 
            { 
                Set(ref discount, value, nameof(Discount));
                if (this.Discount > 0.0)
                {
                    this.TotalPrice = this.TotalPrice - ((this.Discount / 100) * this.TotalPrice);
                }                
            } 
        }

        public double GST { get => gST;
            set 
            { 
                Set(ref gST, value, nameof(GST));
                var tp = (this.Quantity * this.Rate);
                if (this.GST <= 0.0)
                {
                    this.TotalPrice = tp;
                }
                else
                {
                    this.TotalPrice = tp + ((this.GST / 100) * tp);
                }
            }
        }

        public double TotalPrice { get => totalPrice; 
            set 
            { 
                Set(ref totalPrice, value, nameof(TotalPrice));
                var tp = (this.TotalPrice * 100)/(this.GST + 100);
                this.Rate = tp / this.Quantity;
            } 
        }

        public CostModel Clone() { return (CostModel)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

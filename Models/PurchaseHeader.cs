using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class PurchaseHeader
    {
        // Database Primary Key
        public int Id { get; set; }

        // Vendor Link
        public int VendorId { get; set; }

        // Invoice Metadata
        public string InvoiceNo { get; set; }      // Your internal sequence
        public string OriginalInvNo { get; set; }  // The vendor's physical bill number
        public DateTime InvoiceDate { get; set; }

        // Financial Totals (The Sum of all PurchaseItems)
        public double TaxableAmount { get; set; } // Total amount before GST
        public double CGST { get; set; }          // Central Tax total
        public double SGST { get; set; }          // State Tax total
        public double IGST { get; set; }          // Integrated Tax total
        public double GrandTotal { get; set; }    // The final amount to be paid

        // Payment Info
        public string PaymentMode { get; set; }    // Cash/Credit/Cheque
        public double AmountPaid { get; set; }
        public double BalanceAmount { get; set; }
    }
}

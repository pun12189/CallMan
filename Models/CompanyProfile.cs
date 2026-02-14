using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BahiKitab.Models
{
    public class CompanyProfile : ObservableObject
    {
        private string companyName;
        private string proprietor;
        private BitmapSource logoPath;
        private string contact;
        private string email;
        private string gstNumber;
        private string bankName;
        private string accountNumber;
        private string ifscCode;
        private string address;
        private string termsAndConditions;
        private string upiId;
        private string initials;
        private int invno;
        private int id;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }
        public string CompanyName { get => companyName; set => Set(ref companyName, value, nameof(CompanyName)); }
        public string Proprietor { get => proprietor; set => Set(ref proprietor, value, nameof(Proprietor)); }
        public BitmapSource LogoPath { get => logoPath; set => Set(ref logoPath, value, nameof(LogoPath)); }
        public string Contact { get => contact; set => Set(ref contact, value, nameof(Contact)); }
        public string Email { get => email; set => Set(ref email, value, nameof(Email)); }
        public string GstNumber { get => gstNumber; set => Set(ref gstNumber, value, nameof(GstNumber)); }
        public string BankName { get => bankName; set => Set(ref bankName, value, nameof(BankName)); }
        public string AccountNumber { get => accountNumber; set => Set(ref accountNumber, value, nameof(AccountNumber)); }
        public string IfscCode { get => ifscCode; set => Set(ref ifscCode, value, nameof(IfscCode)); }
        public string UpiID { get => upiId; set => Set(ref upiId, value, nameof(UpiID)); }
        public string Address { get => address; set => Set(ref address, value, nameof(Address)); }
        public string Initials { get => initials; set => Set(ref initials, value, nameof(Initials)); }
        public int InvNo { get => invno; set => Set(ref invno, value, nameof(InvNo)); }
        public string TermsAndConditions { get => termsAndConditions; set => Set(ref termsAndConditions, value, nameof(TermsAndConditions)); }
    }
}

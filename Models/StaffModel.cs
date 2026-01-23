using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BahiKitab.Models
{
    public class StaffModel : ObservableObject
    {
        private int id;
        private string fullName;
        private string email;
        private string phone;
        private string username;
        private string password;
        private string address;
        private ImageSource profileImage;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }
        public string FullName { get => fullName; set => Set(ref fullName, value, nameof(FullName)); }
        public string Email { get => email; set => Set(ref email, value, nameof(Email)); }
        public string Phone { get => phone; set => Set(ref phone, value, nameof(Phone)); }
        public string Username { get => username; set => Set(ref username, value, nameof(Username)); }
        public string Password { get => password; set => Set(ref password, value, nameof(Password)); }
        public string Address { get => address; set => Set(ref address, value, nameof(Address)); }
        public ImageSource ProfileImage { get => profileImage; set => Set(ref profileImage, value, nameof(ProfileImage)); }
    }
}

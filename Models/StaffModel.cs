using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BahiKitab.Models
{
    public class StaffModel : ObservableObject, ICloneable
    {
        private int id;
        private string fullName;
        private string email;
        private string phone;
        private string username;
        private string password;
        private string address;
        private ImageSource profileImage;
        private DepartmentsModel department;
        private DateTime createTime;
        private DateTime updateTime;
        private string role;
        private StaffModel teamLead;
        private bool isActive;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }
        public string FullName { get => fullName; set => Set(ref fullName, value, nameof(FullName)); }
        public string Email { get => email; set => Set(ref email, value, nameof(Email)); }
        public string Phone { get => phone; set => Set(ref phone, value, nameof(Phone)); }
        public string Username { get => username; set => Set(ref username, value, nameof(Username)); }
        public string Password { get => password; set => Set(ref password, value, nameof(Password)); }
        public string Address { get => address; set => Set(ref address, value, nameof(Address)); }
        
        [JsonIgnore]
        public ImageSource ProfileImage { get => profileImage; set => Set(ref profileImage, value, nameof(ProfileImage)); }

        public DepartmentsModel Department { get => department; set => Set(ref department, value, nameof(Department)); }
        public DateTime CreateTime { get => createTime; set => Set(ref createTime, value, nameof(CreateTime)); }
        public DateTime UpdateTime { get => updateTime; set => Set(ref updateTime, value, nameof(UpdateTime)); }
        public string Role { get => role; set => Set(ref role, value, nameof(Role)); }
        public StaffModel TeamLead { get => teamLead; set => Set(ref teamLead, value, nameof(TeamLead)); }
        public bool IsActive { get => isActive; set => Set(ref isActive, value, nameof(IsActive)); }

        public StaffModel Clone() { return (StaffModel)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

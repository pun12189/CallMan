using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class SmtpSettings : ObservableObject
    {
        private string host = string.Empty;
        private int port = 587;
        private string senderEmail = string.Empty;
        private string password = string.Empty;
        private bool enableSsl = true;

        public string Host { get => host; set => Set(ref host, value, nameof(Host)); }
        public int Port { get => port; set => Set(ref port, value, nameof(Port)); }
        public string SenderEmail { get => senderEmail; set => Set(ref senderEmail, value, nameof(SenderEmail)); }
        public string Password { get => password; set => Set(ref password, value, nameof(Password)); }
        public bool EnableSsl { get => enableSsl; set => Set(ref enableSsl, value, nameof(EnableSsl)); }
    }
}

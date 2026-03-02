using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> ResetPasswordAsync(string email);
    }
}

using BahiKitab.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordService _passwordService;

        public AuthenticationService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public bool Authenticate(string username, string password)
        {
            // Replace with your DB logic (e.g., using Entity Framework)
            // var user = _context.Users.FirstOrDefault(u => u.Username == username);
            // return user != null && _passwordService.VerifyPassword(password, user.PasswordHash);

            return username == "admin" && password == "admin"; // Mock logic
        }

        public void RequestPasswordReset(string username)
        {
            // Logic to send email/OTP
        }
    }
}

using BahiKitab.Services.Interface;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services
{
    public class BCryptPasswordService : IPasswordService
    {
        // WorkFactor 11 is a good balance between security and performance (approx 250ms per check)
        private const int WorkFactor = 11;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            try
            {
                // BCrypt.Verify is resistant to timing attacks
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (SaltParseException)
            {
                // Handles cases where the stored hash might be malformed or old
                return false;
            }
        }
    }
}

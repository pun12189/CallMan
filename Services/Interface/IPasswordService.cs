using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services.Interface
{
    public interface IPasswordService
    {
        // Returns a secure, salted hash string
        string HashPassword(string password);

        // Compares a plain-text password against a stored hash
        bool VerifyPassword(string password, string hashedPassword);
    }
}

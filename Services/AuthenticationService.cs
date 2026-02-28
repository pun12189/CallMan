using BahiKitab.Models;
using BahiKitab.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            // 1. Find user by username
            var user = await _context.staff.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return false;

            // 2. Use the BCrypt service to verify the plain-text password against the hash
            return await Task.Run(() => _passwordService.VerifyPassword(password, user.Password));
        }        

        public void RequestPasswordReset(string username)
        {
            // Logic to send email/OTP
        }
    }
}

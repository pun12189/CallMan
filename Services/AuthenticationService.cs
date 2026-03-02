using BahiKitab.Models;
using BahiKitab.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _context.staff.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            // 1. Generate a simple temporary password
            string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);

            // 2. Hash and Save
            user.Password = _passwordService.HashPassword(tempPassword);
            await _context.SaveChangesAsync();

            // 3. Send Email (Placeholder for now)
            return await SendEmailAsync(user.Email, tempPassword);
        }

        private async Task<bool> SendEmailAsync(string userEmail, string tempPass)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sofricerp@gmail.com", "oazd ncms rbfa ongy"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sofricerp@gmail.com"),
                    Subject = "Password Reset - BahiKitab",
                    Body = $"<h1>Security Update</h1><p>Your temporary password is: <b>{tempPass}</b></p>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(userEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)  
            { 
                return false;
            }
        }
    }
}

using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BahiKitab.ViewModels
{
    public class EmailSettingsViewModel : ViewModelBase // Assumes INotifyPropertyChanged is implemented
    {
        private SmtpSettings _settings = new();

        public SmtpSettings SmtpSettings { get => _settings; set => Set(ref _settings, value, nameof(SmtpSettings)); }
        
        private readonly EmailSettingsDataService emailSettingsDataService;

        public ICommand SaveCommand { get; }
        public ICommand TestCommand { get; }

        public EmailSettingsViewModel()
        {
            emailSettingsDataService = new EmailSettingsDataService(); 
            SaveCommand = new RelayCommand(ExecuteSave);
            TestCommand = new RelayCommand(ExecuteTest);
            LoadFromDb();
        }

        private void LoadFromDb()
        {
            _settings = emailSettingsDataService.GetEmailSettings();
        }

        private async void ExecuteSave(object passwordContainer)
        {
            var passwordBox = passwordContainer as System.Windows.Controls.PasswordBox;
            string pwd = passwordBox?.Password ?? "";
            SmtpSettings.Password = pwd;
            await emailSettingsDataService.CreateEmailSettingsAsync(SmtpSettings);

            MessageBox.Show("Settings saved successfully.");
        }

        private async void ExecuteTest(object passwordContainer)
        {
            var passwordBox = passwordContainer as System.Windows.Controls.PasswordBox;
            try
            {
                using var client = new SmtpClient(_settings.Host, _settings.Port)
                {
                    Credentials = new NetworkCredential(_settings.SenderEmail, passwordBox?.Password),
                    EnableSsl = _settings.EnableSsl
                };
                await client.SendMailAsync(new MailMessage(_settings.SenderEmail, _settings.SenderEmail, "Test", "Connection Successful!"));
                MessageBox.Show("Test Email Sent!");
            }
            catch (Exception ex) { MessageBox.Show($"Failed: {ex.Message}"); }
        }
    }
}

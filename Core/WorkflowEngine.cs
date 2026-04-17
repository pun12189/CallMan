using BahiKitab.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Core
{
    public class WorkflowEngine
    {
        private readonly WorkflowService _service = new WorkflowService();
        private readonly EmailSettingsDataService _emailSettingsDataService = new EmailSettingsDataService();

        public async Task TriggerEventAsync(string eventKey, object entityData)
        {
            // Get rules from Service
            var rules = await _service.GetActiveRulesForEventAsync(eventKey);

            foreach (var rule in rules)
            {
                string message = ProcessPlaceholders(rule.MessageTemplate, entityData);

                if (rule.SendWhatsApp)
                {
                    // await SendWhatsAppNotification(message, entityData);
                }

                if (rule.SendEmail)
                {
                    await SendEmailNotification(message, entityData);
                }
            }
        }

        private string ProcessPlaceholders(string template, object data)
        {
            if (data == null) return template;

            // Reflection: Works for ANY table/class
            foreach (var prop in data.GetType().GetProperties())
            {
                template = template.Replace($"{{{prop.Name}}}", prop.GetValue(data)?.ToString() ?? "");
            }
            return template;
        }

        public async Task SendEmailNotification(string body, object data)
        {
            // 1. Fetch the settings (including the password) from the DB
            var settings = _emailSettingsDataService.GetEmailSettings();

            if (settings == null) { MessageBox.Show("SMTP Settings not configured."); return; }

            var emailProp = data.GetType().GetProperty("Email") ?? data.GetType().GetProperty("CustomerEmail");
            string recipientEmail = emailProp?.GetValue(data)?.ToString();

            if (string.IsNullOrEmpty(recipientEmail)) return;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SofricONE CRM", settings.SenderEmail));
            message.To.Add(new MailboxAddress("Recipient", recipientEmail));
            message.Subject = "CRM Notification";

            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                // For Gmail/Outlook, use smtp.gmail.com or smtp.office365.com
                await client.ConnectAsync(settings.Host, settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(settings.SenderEmail, settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}

using BahiKitab.Models;
using DocumentFormat.OpenXml.Office.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    public class EmailSettingsDataService : BaseDataService
    {
        public SmtpSettings GetEmailSettings()
        {
            SmtpSettings? lead = new();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM email_settings LIMIT 1";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new SmtpSettings
                            {
                                Host = reader.GetString(1),
                                Port = reader.GetInt32(2),
                                SenderEmail = reader.GetString(3),    
                                Password = reader.GetString(4),
                                EnableSsl = reader.GetBoolean(5)
                            };
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                // Helper.Helper.BugReport(e);
                MessageBox.Show(e.Message);
            }

            return lead;
        }

        public async Task<SmtpSettings> CreateEmailSettingsAsync(SmtpSettings lead)
        {
            await Task.Delay(100);
            // REAL IMPLEMENTATION: Execute INSERT query, retrieve the generated ID (using LAST_INSERT_ID()), set ID on the lead object.

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO email_settings (id, smtp_host, smtp_port, sender_email, smtp_password, enable_ssl) VALUES(1, @h, @p, @e, @pw, @s) ON DUPLICATE KEY UPDATE smtp_host=@h, smtp_port=@p, sender_email=@e, smtp_password=@pw, enable_ssl=@s";
                    command.Parameters.AddWithValue("@h", lead.Host);
                    command.Parameters.AddWithValue("@p", lead.Port);
                    command.Parameters.AddWithValue("@e", lead.SenderEmail);
                    command.Parameters.AddWithValue("@pw", lead.Password); // Note: Encrypt this for production!
                    command.Parameters.AddWithValue("@s", lead.EnableSsl);
                    await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                //Helper.Helper.BugReport(e);
                MessageBox.Show(e.Message);
            }

            return lead;
        }
    }
}

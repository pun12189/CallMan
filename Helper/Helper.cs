using BahiKitab.Models;
using BahiKitab.Services;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BahiKitab.Helper
{
    public static class Helper
    {
        private static readonly EmailSettingsDataService emailSettingsDataService = new EmailSettingsDataService();

        public static string GetSettingsPath(string filename)
        {
            // Path: C:\Users\<User>\AppData\Roaming\YourAppName
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BahiKitab"
            );

            // Ensure the folder exists before we try to write to it
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return Path.Combine(folder, filename);
        }

        public static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }

        public static DataTable ConvertCsvToDataTable(string filePath)
        {
            DataTable dtData = new DataTable();
            try
            {
                //reading all the lines(rows) from the file.
                string[] rows = File.ReadAllLines(filePath);
                string[] rowValues = null;
                DataRow dr = dtData.NewRow();

                //Creating columns
                if (rows.Length > 0)
                {
                    foreach (string columnName in rows[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                        dtData.Columns.Add(columnName);
                }
                
                //Creating row for each line.(except the first line, which contain column names)
                for (int row = 1; row < rows.Length; row++)
                {
                    var rowStr = new string[4];
                    rowValues = rows[row].Split(',');
                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        rowStr[i] = rowValues[i].Trim();
                    }

                    dr = dtData.NewRow();
                    dr.ItemArray = rowStr;
                    dtData.Rows.Add(dr);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Either the file is open or access by another process or app. " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //Helper.LogError(e);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show("Either the file is open or access by another process or app. " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //Helper.LogError(e);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //Helper.LogError(e);
            }

            return dtData;
        }

        public static async Task BulkUpdateDataAsync(System.Data.DataTable dt)
        {
            var _connectionString = string.Empty;
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab;AllowLoadLocalInfile=true";
#endif
#if RELEASE

            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab;AllowLoadLocalInfile=true";
#endif
#if TESTING

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand("CREATE TABLE tmptable (`name` varchar(100), `email` varchar(100), `phone` text, `leadType` varchar(100) PRIMARY KEY (`phone`))", conn))
                {
                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();

                        var bulkCopy = new MySqlBulkCopy(conn);
                        bulkCopy.DestinationTableName = "tmptable";
                        var result = await bulkCopy.WriteToServerAsync(dt);

                        command.CommandTimeout = 3000;
                        command.CommandText = "UPDATE leads INNER JOIN tmptable ON actives.code = tmptable.code SET actives.stocks = tmptable.stocks WHERE tmptable.code = actives.code OR tmptable.name = actives.name; DROP TABLE tmptable";
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bulk Update Failed: " + ex.Message, "Bulk Update", MessageBoxButton.OK, MessageBoxImage.Error);
                        using var cmd = new MySqlCommand("DROP TABLE tmptable;");
                        try
                        {
                            conn.Open();
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    finally
                    {
                        conn.Close();
                        MessageBox.Show("Bulk Update runs successfully. Please check the inventory by clicking on refresh button.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        public static byte[] ImageToByteArray(ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static BitmapImage ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                // Create a Bitmap from the stream
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.CacheOption = BitmapCacheOption.OnLoad;
                bm.StreamSource = ms;
                bm.EndInit();
                return bm;
            }
        }

        public static string SaveBitmapSourceToTempFile(BitmapSource bitmapSource)
        {
            // एक अस्थायी फाइल पाथ बनाएँ
            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");

            using (var fileStream = new System.IO.FileStream(tempPath, System.IO.FileMode.Create))
            {
                // PNG के रूप में एनकोड करें
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }

            return tempPath;
        }

        public static async Task ProcessBulkEmailsAsync(List<Lead> leads, string subject, string body, string filePath)
        {
            var settings = emailSettingsDataService.GetEmailSettings(); // MySQL fetch

            foreach (var lead in leads)
            {
                if (!string.IsNullOrEmpty(lead.Email) || !string.IsNullOrWhiteSpace(lead.Email))
                {
                    try
                    {
                        using var mail = new MailMessage();
                        mail.From = new MailAddress(settings.SenderEmail);
                        mail.To.Add(lead.Email);
                        mail.Subject = subject;
                        mail.Body = body.Replace("[Name]", lead.Name);

                        if (!string.IsNullOrEmpty(filePath))
                            mail.Attachments.Add(new Attachment(filePath));

                        using var smtp = new SmtpClient(settings.Host, settings.Port)
                        {
                            Credentials = new NetworkCredential(settings.SenderEmail, settings.Password),
                            EnableSsl = settings.EnableSsl
                        };

                        await smtp.SendMailAsync(mail);

                        // Wait 3 seconds to stay under the "Spam" radar
                        await Task.Delay(3000);
                    }
                    catch (Exception ex)
                    {
                        // Since there is no UI, log errors to a file or a "Log" table in MySQL
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}

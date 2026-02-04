using BahiKitab.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BahiKitab.Services
{
    public class ImagesDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public async Task<ImageModel> GetImage(int id)
        {
            ImageModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from UserImages where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            byte[] rawData = (byte[])reader["ImageData"];
                            lead = new ImageModel
                            {
                                Id = reader.GetInt32(0),
                                FileName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),                                
                                ImageSource = ToImage(rawData),
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

        public async Task<ObservableCollection<ImageModel>> GetAllImagesAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<ImageModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from UserImages";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<ImageModel>();
                        while (reader.Read())
                        {
                            byte[] rawData = (byte[])reader["ImageData"];
                            var lead = new ImageModel
                            {
                                Id = reader.GetInt32(0),
                                FileName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ImageSource = reader.IsDBNull(2) ? null : ToImage(rawData),
                            };

                            leads.Add(lead);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //Helper.Helper.BugReport(e);
            }

            return leads;
        }

        public async Task<int> CreateImageAsync(string fileName, byte[] imageBytes)
        {
            await Task.Delay(100);
            int id = 0;
            // REAL IMPLEMENTATION: Execute INSERT query, retrieve the generated ID (using LAST_INSERT_ID()), set ID on the lead object.

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into UserImages(FileName, ImageData) values(@name, @data); select LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@name", fileName);
                    command.Parameters.AddWithValue("@data", imageBytes); // Binary data handled here

                    id = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
            catch (Exception e)
            {
                //Helper.Helper.BugReport(e);
                MessageBox.Show(e.Message);
            }

            return id;
        }

        public async Task<ImageModel> UpdateDepartmentsAsync(ImageModel lead)
        {
            await Task.Delay(100);
            // REAL IMPLEMENTATION: Execute an UPDATE query using lead.Id to identify the record.

            // Mock: Update the item in the mock collection
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update UserImages set FileName=@name, ImageData=@data where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.FileName;
                    command.Parameters.Add("@data", MySqlDbType.Byte).Value = lead.ImageSource;
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

        public async Task DeleteDepartmentsAsync(int id)
        {
            await Task.Delay(100);
            // REAL IMPLEMENTATION: Execute a DELETE query using lead.Id.

            // Mock:
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from UserImages where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private BitmapSource ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // Prevents memory leaks
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // Makes it cross-thread accessible
                return image;
            }
        }
    }
}

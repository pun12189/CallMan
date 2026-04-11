using BahiKitab.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    public class VendorDataService : BaseDataService
    {
        public async Task<VendorModel> GetVendor(int id)
        {
            VendorModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from vendors where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {                            
                            lead = new VendorModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Contact = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                GSTIN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
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

        public async Task<ObservableCollection<VendorModel>> GetAllVendorsAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<VendorModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from vendors";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<VendorModel>();
                        while (reader.Read())
                        {
                            var lead = new VendorModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Contact = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                GSTIN = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
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

        public async Task<int> CreateVendorAsync(VendorModel vm)
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
                    command.CommandText = "INSERT INTO vendors(name, address, contact, gstin) values(@name, @address, @contact, @gstin); select LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@name", vm.Name);
                    command.Parameters.AddWithValue("@address", vm.Address);
                    command.Parameters.AddWithValue("@contact", vm.Contact);
                    command.Parameters.AddWithValue("@gstin", vm.GSTIN);// Binary data handled here

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

        public async Task<VendorModel> UpdateDepartmentsAsync(VendorModel vm)
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
                    command.CommandText = "update vendors set name=@name, address=@address, contact=@contact, gstin=@gstin where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = vm.Id;
                    command.Parameters.AddWithValue("@name", vm.Name);
                    command.Parameters.AddWithValue("@address", vm.Address);
                    command.Parameters.AddWithValue("@contact", vm.Contact);
                    command.Parameters.AddWithValue("@gstin", vm.GSTIN);
                    await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                //Helper.Helper.BugReport(e);
                MessageBox.Show(e.Message);
            }

            return vm;
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
                    command.CommandText = "delete from vendors where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

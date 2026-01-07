using BahiKitab.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    public class LeadStatusDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public LeadStatusModel GetLeadStatus(int id)
        {
            LeadStatusModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_status where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new LeadStatusModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CreationDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
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

        public async Task<ObservableCollection<LeadStatusModel>> GetAllLeadStatussAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<LeadStatusModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_status";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<LeadStatusModel>();
                        while (reader.Read())
                        {
                            var lead = new LeadStatusModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CreationDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
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

        public async Task<LeadStatusModel> CreateLeadStatusAsync(LeadStatusModel lead)
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
                    command.CommandText = "insert into lead_status(name) values(@name)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
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

        public async Task<LeadStatusModel> UpdateLeadStatusAsync(LeadStatusModel lead)
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
                    command.CommandText = "update lead_status set name=@name where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
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

        public async Task DeleteLeadStatusAsync(LeadStatusModel lead)
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
                    command.CommandText = "delete from lead_status where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
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

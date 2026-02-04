using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace BahiKitab.Services
{
    public class LeadHistoryDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public LeadHistoryModel GetLeadHistory(int id)
        {
            LeadHistoryModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_history where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new LeadHistoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CreationDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                LeadId = reader.GetInt32(3),
                                LeadType = reader.IsDBNull(4) ? LeadType.New : Enum.Parse<LeadType>(reader.GetString(4), true),
                                LastMsg = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                NextFollowUp = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(6),
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

        public async Task<ObservableCollection<LeadHistoryModel>> GetLeadHistoryByLeadId(int id)
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<LeadHistoryModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_history where lead_id=@lead_id";
                    command.Parameters.Add("@lead_id", MySqlDbType.Int32).Value = id;
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<LeadHistoryModel>();
                        while (reader.Read())
                        {
                            var lead = new LeadHistoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CreationDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                LeadId = reader.GetInt32(3),
                                LeadType = reader.IsDBNull(4) ? LeadType.New : Enum.Parse<LeadType>(reader.GetString(4), true),
                                LastMsg = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                NextFollowUp = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(6),
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

        public async Task<ObservableCollection<LeadHistoryModel>> GetAllLeadHistorysAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<LeadHistoryModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_history";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<LeadHistoryModel>();
                        while (reader.Read())
                        {
                            var lead = new LeadHistoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                CreationDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                LeadId = reader.GetInt32(3),
                                LeadType = reader.IsDBNull(4) ? LeadType.New : Enum.Parse<LeadType>(reader.GetString(4), true),
                                LastMsg = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                NextFollowUp = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(6),
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

        public async Task<LeadHistoryModel> CreateLeadHistoryAsync(LeadHistoryModel lead)
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
                    command.CommandText = "insert into lead_history(name, lead_id, leadType, lastMsg, nextFollowup) values(@name, @lead_id, @leadType, @lastMsg, @nextFollowup)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@lead_id", MySqlDbType.Int32).Value = lead.LeadId;
                    command.Parameters.Add("@leadType", MySqlDbType.VarChar).Value = lead.LeadType;
                    command.Parameters.Add("@lastMsg", MySqlDbType.String).Value = lead.LastMsg;
                    command.Parameters.Add("@nextFollowup", MySqlDbType.DateTime).Value = lead.NextFollowUp;
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

        public async Task<LeadHistoryModel> UpdateLeadHistoryAsync(LeadHistoryModel lead)
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
                    command.CommandText = "update lead_history set name=@name, leadType=@leadType, lastMsg=@lastMsg, nextFollowup=@nextFollowup where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@leadType", MySqlDbType.VarChar).Value = lead.LeadType;
                    command.Parameters.Add("@lastMsg", MySqlDbType.String).Value = lead.LastMsg;
                    command.Parameters.Add("@nextFollowup", MySqlDbType.DateTime).Value = lead.NextFollowUp;
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

        public async Task DeleteLeadHistoryAsync(LeadHistoryModel lead)
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
                    command.CommandText = "delete from lead_history where id=@id";
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

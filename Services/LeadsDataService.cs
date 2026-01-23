using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    // The Data Service is responsible for all interactions with the MySQL database.
    public class LeadsDataService : BaseDataService
    {
        // Mock database storage
        private ObservableCollection<Lead> _mockLeads = new ObservableCollection<Lead>();
        private int _nextId = 1;

        // NOTE: This connection string must be configured correctly in a production environment
        private const string CONNECTION_STRING = "Server=localhost;Database=bahikitab;Uid=root;Pwd='';";

        //public LeadsDataService()
        //{
        //    // Add some mock data
        //    CreateLeadAsync(new Lead { Id = _nextId++, FirstName = "Alice", LastName = "Smith", Company = "Tech Innovations", Email = "alice@example.com", Phone = "8787545825", Stage = "New" });
        //    CreateLeadAsync(new Lead { Id = _nextId++, FirstName = "Bob", LastName = "Johnson", Company = "Global Corp", Email = "bob@example.com", Phone = "8545587552", Stage = "Qualified" });
        //}

        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public Lead GetCustomer(int id)
        {
            Lead? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from leads where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new Lead
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Company = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Stage = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                LeadSource = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<LeadSourceModel>(reader.GetString(6)),
                                Tags = reader.IsDBNull(7) ? null : JsonSerializer.Deserialize<LeadTagModel>(reader.GetString(7)),
                                Label = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<LeadLabelsModel>(reader.GetString(8)),
                                Country = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                State = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                City = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                District = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Pincode = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                FirmName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                ImpDate = reader.IsDBNull(15) ? DateTime.MinValue : reader.GetDateTime(15),
                                AltPhone = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                CreationDate = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                                UpdationDate = reader.IsDBNull(18) ? DateTime.MinValue : reader.GetDateTime(18),
                                Status = reader.IsDBNull(19) ? null : JsonSerializer.Deserialize<LeadStatusModel>(reader.GetString(19)),
                                LeadType = reader.IsDBNull(20) ? LeadType.New : Enum.Parse<LeadType>(reader.GetString(20)),
                                LeadFollowUpModel = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<LeadFollowUpModel>(reader.GetString(21)),
                                LeadDeadModel = reader.IsDBNull(22) ? null : JsonSerializer.Deserialize<LeadDeadModel>(reader.GetString(22)),
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

        public async Task<ObservableCollection<Lead>> GetAllLeadsAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<Lead> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from leads";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<Lead>();
                        while (reader.Read())
                        {
                            var lead = new Lead
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Company = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Phone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Stage = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<string>(reader.GetString(5)),
                                LeadSource = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<LeadSourceModel>(reader.GetString(6)),
                                Tags = reader.IsDBNull(7) ? null : JsonSerializer.Deserialize<LeadTagModel>(reader.GetString(7)),
                                Label = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<LeadLabelsModel>(reader.GetString(8)),
                                Country = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                State = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                City = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                District = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Pincode = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                FirmName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                ImpDate = reader.IsDBNull(15) ? DateTime.MinValue : reader.GetDateTime(15),
                                AltPhone = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                CreationDate = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                                UpdationDate = reader.IsDBNull(18) ? DateTime.MinValue : reader.GetDateTime(18),
                                Status = reader.IsDBNull(19) ? null : JsonSerializer.Deserialize<LeadStatusModel>(reader.GetString(19)),
                                LeadType = reader.IsDBNull(20) ? LeadType.New : Enum.Parse<LeadType>(reader.GetString(20)),
                                LeadFollowUpModel = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<LeadFollowUpModel>(reader.GetString(21)),
                                LeadDeadModel = reader.IsDBNull(22) ? null : JsonSerializer.Deserialize<LeadDeadModel>(reader.GetString(22)),
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

        public async Task<Lead> CreateLeadAsync(Lead lead)
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
                    command.CommandText = "insert into leads(name, company, email, phone, stage, leadSource, tags, label, country, state, city, district, pincode, firm_name, imp_date, alternate_number, status, leadType, leadFollowup, leadDead) values(@name, @company, @email, @phone, @stage, @leadSource, @tags, @label, @country, @state, @city, @district, @pincode, @firm_name, @imp_date, @alternate_number, @status, @leadType, @leadFollowup, @leadDead)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@company", MySqlDbType.VarChar).Value = lead.Company;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = lead.Phone;
                    command.Parameters.Add("@stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Stage);
                    command.Parameters.Add("@leadSource", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadSource);
                    command.Parameters.Add("@tags", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Tags);
                    command.Parameters.Add("@label", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Label);
                    command.Parameters.Add("@country", MySqlDbType.VarChar).Value = lead.Country;
                    command.Parameters.Add("@state", MySqlDbType.VarChar).Value = lead.State;
                    command.Parameters.Add("@city", MySqlDbType.VarChar).Value = lead.City;
                    command.Parameters.Add("@district", MySqlDbType.VarChar).Value = lead.District;
                    command.Parameters.Add("@pincode", MySqlDbType.VarChar).Value = lead.Pincode;
                    command.Parameters.Add("@firm_name", MySqlDbType.VarChar).Value = lead.FirmName;
                    command.Parameters.Add("@imp_date", MySqlDbType.DateTime).Value = lead.ImpDate;
                    command.Parameters.Add("@alternate_number", MySqlDbType.VarChar).Value = lead.AltPhone;
                    command.Parameters.Add("@status", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Status);
                    command.Parameters.Add("@leadType", MySqlDbType.VarChar).Value = lead.LeadType;
                    command.Parameters.Add("@leadFollowup", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadFollowUpModel);
                    command.Parameters.Add("@leadDead", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadDeadModel);
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

        public async Task<Lead> UpdateLeadAsync(Lead lead)
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
                    command.CommandText = "update leads set name=@name, company=@company, email=@email, phone=@phone, stage=@stage, leadSource=@leadSource, tags=@tags, label=@label, updated_date=@updated_date, country=@country, state=@state, city=@city, district=@district, pincode=@pincode, firm_name=@firm_name, imp_date=@imp_date, alternate_number=@alternate_number, status=@status, leadType=@leadType, leadFollowup=@leadFollowup, leadDead=@leadDead where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@company", MySqlDbType.VarChar).Value = lead.Company;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = lead.Phone;
                    command.Parameters.Add("@stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Stage);
                    command.Parameters.Add("@leadSource", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadSource);
                    command.Parameters.Add("@tags", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Tags);
                    command.Parameters.Add("@label", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Label);
                    command.Parameters.Add("@updated_date", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@country", MySqlDbType.VarChar).Value = lead.Country;
                    command.Parameters.Add("@state", MySqlDbType.VarChar).Value = lead.State;
                    command.Parameters.Add("@city", MySqlDbType.VarChar).Value = lead.City;
                    command.Parameters.Add("@district", MySqlDbType.VarChar).Value = lead.District;
                    command.Parameters.Add("@pincode", MySqlDbType.VarChar).Value = lead.Pincode;
                    command.Parameters.Add("@firm_name", MySqlDbType.VarChar).Value = lead.FirmName;
                    command.Parameters.Add("@imp_date", MySqlDbType.DateTime).Value = lead.ImpDate;
                    command.Parameters.Add("@alternate_number", MySqlDbType.VarChar).Value = lead.AltPhone;
                    command.Parameters.Add("@status", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Status);
                    command.Parameters.Add("@leadType", MySqlDbType.VarChar).Value = lead.LeadType;
                    command.Parameters.Add("@leadFollowup", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadFollowUpModel);
                    command.Parameters.Add("@leadDead", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.LeadDeadModel);
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

        public async Task DeleteLeadAsync(Lead lead)
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
                    command.CommandText = "delete from leads where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    await command.ExecuteScalarAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public async Task BulkInsertMySQL(DataTable table, string tableName)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    using (MySqlTransaction tran = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.Transaction = tran;
                            cmd.CommandText = $"SELECT name, company, email, phone, leadType FROM " + tableName + " limit 0";

                            try
                            {
                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                                {
                                    adapter.UpdateBatchSize = 10000;
                                    using (MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter))
                                    {
                                        cb.SetAllValues = true;
                                        adapter.Update(table);
                                        await tran.CommitAsync();
                                    }
                                }
                                ;
                            }
                            catch (MySqlException e)
                            {
                                MessageBox.Show("CSV File must have unique data", "Info");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Helper.Helper.BugReport(e);
            }
        }
    }

}

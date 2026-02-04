using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    public class StaffDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public StaffModel GetStaff(int id)
        {
            StaffModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from staff where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new StaffModel
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Role = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Username = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                Password = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                Department = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<DepartmentsModel>(reader.GetString(8)),
                                TeamLead = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<StaffModel>(reader.GetString(9)),
                                IsActive = reader.IsDBNull(10) ? true : reader.GetBoolean(10),
                                ProfileImage = reader.IsDBNull(11) ? null : Helper.Helper.ByteArrayToImage((byte[])reader["profileImage"]),
                                CreateTime = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12),
                                UpdateTime = reader.IsDBNull(13) ? DateTime.MinValue : reader.GetDateTime(13),
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

        public async Task<ObservableCollection<StaffModel>> GetAllStaffAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<StaffModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from staff";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<StaffModel>();
                        while (reader.Read())
                        {
                            var lead = new StaffModel
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Phone = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Role = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                Username = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                Password = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                Department = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<DepartmentsModel>(reader.GetString(8)),
                                TeamLead = reader.IsDBNull(9) ? null : JsonSerializer.Deserialize<StaffModel>(reader.GetString(9)),
                                IsActive = reader.IsDBNull(10) ? true : reader.GetBoolean(10),
                                ProfileImage = reader.IsDBNull(11) ? null : Helper.Helper.ByteArrayToImage((byte[])reader["profileImage"]),
                                CreateTime = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12),
                                UpdateTime = reader.IsDBNull(13) ? DateTime.MinValue : reader.GetDateTime(13),
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

        public async Task<StaffModel> CreateStaffAsync(StaffModel lead)
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
                    command.CommandText = "insert into staff(name, email, phone, address, role, uname, password, department, team_lead, status, profileImage) values(@name, @email, @phone, @address, @role, @uname, @password, @department, @team_lead, @status, @profileImage)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.FullName;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = lead.Phone;
                    command.Parameters.Add("@address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@role", MySqlDbType.VarChar).Value = lead.Role;
                    command.Parameters.Add("@uname", MySqlDbType.VarChar).Value = lead.Username;
                    command.Parameters.Add("@password", MySqlDbType.VarString).Value = lead.Password;
                    command.Parameters.Add("@department", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Department);
                    command.Parameters.Add("@team_lead", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.TeamLead);
                    command.Parameters.Add("@status", MySqlDbType.Bit).Value = lead.IsActive;
                    command.Parameters.Add("@profileImage", MySqlDbType.LongBlob).Value = Helper.Helper.ImageToByteArray(lead.ProfileImage);
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

        public async Task<StaffModel> UpdateStaffAsync(StaffModel lead)
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
                    command.CommandText = "update staff set name=@name, email=@email, phone=@phone, address=@address, role=@role, uname=@uname, password=@password, department=@department, team_lead=@team_lead, status=@status, profileImage=@profileImage, update_time=@update_time where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.FullName;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = lead.Phone;
                    command.Parameters.Add("@address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@role", MySqlDbType.VarChar).Value = lead.Role;
                    command.Parameters.Add("@uname", MySqlDbType.VarChar).Value = lead.Username;
                    command.Parameters.Add("@password", MySqlDbType.VarString).Value = lead.Password;
                    command.Parameters.Add("@department", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Department);
                    command.Parameters.Add("@team_lead", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.TeamLead);
                    command.Parameters.Add("@status", MySqlDbType.Bit).Value = lead.IsActive;
                    command.Parameters.Add("@profileImage", MySqlDbType.LongBlob).Value = Helper.Helper.ImageToByteArray(lead.ProfileImage);
                    command.Parameters.Add("@update_time", MySqlDbType.DateTime).Value = DateTime.Now;
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

        public async Task DeleteStaffAsync(StaffModel lead)
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
                    command.CommandText = "delete from staff where id=@id";
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

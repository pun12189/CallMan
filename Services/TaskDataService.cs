using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
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
    public class TaskDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public TaskModel GetTask(int id)
        {
            TaskModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from tasks where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new TaskModel
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<Lead>(reader.GetString(2)),
                                Staff = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<ObservableCollection<StaffModel>>(reader.GetString(3)),
                                Departments = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<ObservableCollection<DepartmentsModel>>(reader.GetString(4)),
                                Product = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<ProductModel>(reader.GetString(5)),
                                IsAccepted = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                Remarks = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
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

        public async Task<ObservableCollection<TaskModel>> GetAllLeadHistorysAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<TaskModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from tasks";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<TaskModel>();
                        while (reader.Read())
                        {
                            var lead = new TaskModel
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : JsonSerializer.Deserialize<Lead>(reader.GetString(2)),
                                Staff = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<ObservableCollection<StaffModel>>(reader.GetString(3)),
                                Departments = reader.IsDBNull(4) ? null : JsonSerializer.Deserialize<ObservableCollection<DepartmentsModel>>(reader.GetString(4)),
                                Product = reader.IsDBNull(5) ? null : JsonSerializer.Deserialize<ProductModel>(reader.GetString(5)),
                                IsAccepted = reader.IsDBNull(6) ? false : reader.GetBoolean(6),
                                Remarks = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
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

        public async Task<TaskModel> CreateLeadHistoryAsync(TaskModel lead)
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
                    command.CommandText = "insert into tasks(orderid, customer, staffs, departments, product, isaccepted, remarks) values(@orderid, @customer, @staffs, @departments, @product, @isaccepted, @remarks)";
                    command.Parameters.Add("@orderid", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@customer", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Customer);
                    command.Parameters.Add("@staffs", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Staff);
                    command.Parameters.Add("@departments", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Departments);
                    command.Parameters.Add("@product", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Product);
                    command.Parameters.Add("@isaccepted", MySqlDbType.Bit).Value = lead.IsAccepted;
                    command.Parameters.Add("@remarks", MySqlDbType.String).Value = lead.Remarks;
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

        public async Task<TaskModel> UpdateLeadHistoryAsync(TaskModel lead)
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
                    command.CommandText = "update tasks set orderid=@orderid, customer=@customer, staffs=@staffs, departments=@departments, product=@product, isaccepted=@isaccepted, remarks=@remarks where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@orderid", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@customer", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Customer);
                    command.Parameters.Add("@staffs", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Staff);
                    command.Parameters.Add("@departments", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Departments);
                    command.Parameters.Add("@product", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Product);
                    command.Parameters.Add("@isaccepted", MySqlDbType.Bit).Value = lead.IsAccepted;
                    command.Parameters.Add("@remarks", MySqlDbType.String).Value = lead.Remarks;
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

        public async Task DeleteLeadHistoryAsync(TaskModel lead)
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
                    command.CommandText = "delete from tasks where id=@id";
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

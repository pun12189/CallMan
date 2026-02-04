using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
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
    // The Data Service is responsible for all interactions with the MySQL database.
    public class LeadsOrderDataService : BaseDataService
    {
        // Mock database storage
        private ObservableCollection<Lead> _mockLeads = new ObservableCollection<Lead>();
        private LeadsDataService LeadsDataService = new LeadsDataService();
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

        public LeadOrderModel GetOrder(int id)
        {
            LeadOrderModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_orders where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new LeadOrderModel
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                OrderAmount = reader.IsDBNull(2) ? 0.0 : reader.GetDouble(2),
                                ReceivedAmount = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                Customer = reader.IsDBNull(4) ? null : LeadsDataService.GetCustomer(reader.GetInt32(4)),
                                PaymentType = reader.IsDBNull(5) ? PaymentType.Credit : Enum.Parse<PaymentType>(reader.GetString(5)),
                                PaymentStatus= reader.IsDBNull(6) ? PaymentStatus.Unpaid : Enum.Parse<PaymentStatus>(reader.GetString(6)),
                                OrderStatus = reader.IsDBNull(7) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(7)),
                                OrderedProducts = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<ObservableCollection<ProductModel>>(reader.GetString(8)),
                                NextFollowup = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                                AcceptedDate = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
                                IsAccepted = reader.IsDBNull(11) ? false : reader.GetBoolean(11),
                                TakenBy = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<StaffModel>(reader.GetString(12)),
                                Discount = reader.IsDBNull(13) ? 0.0 : reader.GetDouble(13),
                                Balance = reader.IsDBNull(14) ? 0.0 : reader.GetDouble(14),
                                Priority = reader.IsDBNull(15) ? false : reader.GetBoolean(15),
                                Created = reader.IsDBNull(16) ? DateTime.MinValue : reader.GetDateTime(16),
                                Updated = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                                LastMsg = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
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

        public async Task<ObservableCollection<LeadOrderModel>> GetAllOrdersAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<LeadOrderModel> leads = new ObservableCollection<LeadOrderModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from lead_orders";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var lead = new LeadOrderModel
                            {
                                Id = reader.GetInt32(0),
                                OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                OrderAmount = reader.IsDBNull(2) ? 0.0 : reader.GetDouble(2),
                                ReceivedAmount = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                Customer = reader.IsDBNull(4) ? null : LeadsDataService.GetCustomer(reader.GetInt32(4)),
                                PaymentType = reader.IsDBNull(5) ? PaymentType.Credit : Enum.Parse<PaymentType>(reader.GetString(5)),
                                PaymentStatus = reader.IsDBNull(6) ? PaymentStatus.Unpaid : Enum.Parse<PaymentStatus>(reader.GetString(6)),
                                OrderStatus = reader.IsDBNull(7) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(7)),
                                OrderedProducts = reader.IsDBNull(8) ? null : JsonSerializer.Deserialize<ObservableCollection<ProductModel>>(reader.GetString(8)),
                                NextFollowup = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                                AcceptedDate = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
                                IsAccepted = reader.IsDBNull(11) ? false : reader.GetBoolean(11),
                                TakenBy = reader.IsDBNull(12) ? null : JsonSerializer.Deserialize<StaffModel>(reader.GetString(12)),
                                Discount = reader.IsDBNull(13) ? 0.0 : reader.GetDouble(13),
                                Balance = reader.IsDBNull(14) ? 0.0 : reader.GetDouble(14),
                                Priority = reader.IsDBNull(15) ? false : reader.GetBoolean(15),
                                Created = reader.IsDBNull(16) ? DateTime.MinValue : reader.GetDateTime(16),
                                Updated = reader.IsDBNull(17) ? DateTime.MinValue : reader.GetDateTime(17),
                                LastMsg = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
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

        public async Task<LeadOrderModel> CreateLeadOrderAsync(LeadOrderModel lead)
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
                    command.CommandText = "insert into lead_orders(order_id, order_amt, recvd_amt, lead_id, payment_type, payment_status, order_stage, products, next_followup, accepted_date, isAccepted, takenby, discount, balance, priority, lastMsg) values(@order_id, @order_amt, @recvd_amt, @lead_id, @payment_type, @payment_status, @order_stage, @products, @next_followup, @accepted_date, @isAccepted, @takenby, @discount, @balance, @priority, @lastMsg)";
                    command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@order_amt", MySqlDbType.Double).Value = lead.OrderAmount;
                    command.Parameters.Add("@recvd_amt", MySqlDbType.Double).Value = lead.ReceivedAmount;
                    command.Parameters.Add("@lead_id", MySqlDbType.Int32).Value = lead.Customer.Id;
                    command.Parameters.Add("@payment_type", MySqlDbType.VarChar).Value = lead.PaymentType;
                    command.Parameters.Add("@payment_status", MySqlDbType.VarChar).Value = lead.PaymentStatus;
                    command.Parameters.Add("@order_stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderStatus);
                    command.Parameters.Add("@products", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderedProducts);
                    command.Parameters.Add("@next_followup", MySqlDbType.DateTime).Value = lead.NextFollowup;
                    command.Parameters.Add("@accepted_date", MySqlDbType.DateTime).Value = lead.AcceptedDate;
                    command.Parameters.Add("@isAccepted", MySqlDbType.Byte).Value = lead.IsAccepted;
                    command.Parameters.Add("@takenby", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.TakenBy);
                    command.Parameters.Add("@discount", MySqlDbType.Double).Value = lead.Discount;
                    command.Parameters.Add("@balance", MySqlDbType.Double).Value = lead.Balance;
                    command.Parameters.Add("@priority", MySqlDbType.Byte).Value = lead.Priority;
                    command.Parameters.Add("@lastMsg", MySqlDbType.String).Value = lead.LastMsg;
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

        public async Task<LeadOrderModel> UpdateLeadOrderAsync(LeadOrderModel lead)
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
                    command.CommandText = "update lead_orders set order_id=@order_id, order_amt=@order_amt, recvd_amt=@recvd_amt, lead_id=@lead_id, payment_type=@payment_type, payment_status=@payment_status, order_stage=@order_stage, products=@products, next_followup=@next_followup, accepted_date=@accepted_date, isAccepted=@isAccepted, takenby=@takenby, discount=@discount, balance=@balance, priority=@priority, create_time=@create_time, update_time=@update_time, lastMsg=@lastMsg where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@order_amt", MySqlDbType.Double).Value = lead.OrderAmount;
                    command.Parameters.Add("@recvd_amt", MySqlDbType.Double).Value = lead.ReceivedAmount;
                    command.Parameters.Add("@lead_id", MySqlDbType.Int32).Value = lead.Customer.Id;
                    command.Parameters.Add("@payment_type", MySqlDbType.VarChar).Value = lead.PaymentType;
                    command.Parameters.Add("@payment_status", MySqlDbType.VarChar).Value = lead.PaymentStatus;
                    command.Parameters.Add("@order_stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderStatus);
                    command.Parameters.Add("@products", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderedProducts);
                    command.Parameters.Add("@next_followup", MySqlDbType.DateTime).Value = lead.NextFollowup;
                    command.Parameters.Add("@accepted_date", MySqlDbType.DateTime).Value = lead.AcceptedDate;
                    command.Parameters.Add("@isAccepted", MySqlDbType.Byte).Value = lead.IsAccepted;
                    command.Parameters.Add("@takenby", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.TakenBy);
                    command.Parameters.Add("@discount", MySqlDbType.Double).Value = lead.Discount;
                    command.Parameters.Add("@balance", MySqlDbType.Double).Value = lead.Balance;
                    command.Parameters.Add("@priority", MySqlDbType.Byte).Value = lead.Priority;
                    command.Parameters.Add("@lastMsg", MySqlDbType.String).Value = lead.LastMsg;
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

        public async Task DeleteLeadOrderAsync(LeadOrderModel lead)
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
                    command.CommandText = "delete from lead_orders where id=@id";
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

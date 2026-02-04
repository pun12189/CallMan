using BahiKitab.Helper;
using BahiKitab.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.Services
{
    public class InventoryDataService : BaseDataService
    {
        // ==============================================================================
        // CRUD Operations (Mocked implementations)
        // ==============================================================================

        public InventoryModel GetInventory(int id)
        {
            InventoryModel? lead = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from inventory where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new InventoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ShortCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Stock = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                SKU = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4),
                                Units = reader.IsDBNull(5) ? ProductUnits.pcs : Enum.Parse<ProductUnits>(reader.GetString(5), true),
                                GST = reader.IsDBNull(6) ? 0.0 : reader.GetDouble(6),
                                SellingPrice = reader.IsDBNull(7) ? 0.0 : reader.GetDouble(7),
                                PurchasePrice = reader.IsDBNull(8) ? 0.0 : reader.GetDouble(8),
                                Created = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                                Updated = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
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

        public async Task<ObservableCollection<InventoryModel>> GetAllInventoryAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to Lead objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<InventoryModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from inventory";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<InventoryModel>();
                        while (reader.Read())
                        {
                            var lead = new InventoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ShortCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Stock = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                                SKU = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4),
                                Units = reader.IsDBNull(5) ? ProductUnits.pcs : Enum.Parse<ProductUnits>(reader.GetString(5), true),
                                GST = reader.IsDBNull(6) ? 0.0 : reader.GetDouble(6),
                                SellingPrice = reader.IsDBNull(7) ? 0.0 : reader.GetDouble(7),
                                PurchasePrice = reader.IsDBNull(8) ? 0.0 : reader.GetDouble(8),
                                Created = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                                Updated = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
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

        public async Task<InventoryModel> CreateInventoryAsync(InventoryModel lead)
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
                    command.CommandText = "insert into inventory(name, code, stock, sku, units, gst, saleprice, purchaseprice) values(@name, @code, @stock, @sku, @units, @gst, @saleprice, @purchaseprice)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.ShortCode;
                    command.Parameters.Add("@stock", MySqlDbType.Double).Value = lead.Stock;
                    command.Parameters.Add("@sku", MySqlDbType.Double).Value = lead.SKU;
                    command.Parameters.Add("@units", MySqlDbType.VarChar).Value = lead.Units;
                    command.Parameters.Add("@gst", MySqlDbType.Double).Value = lead.GST;
                    command.Parameters.Add("@saleprice", MySqlDbType.Double).Value = lead.SellingPrice;
                    command.Parameters.Add("@purchaseprice", MySqlDbType.Double).Value = lead.PurchasePrice;                    
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

        public async Task<InventoryModel> UpdateInventoryAsync(InventoryModel lead)
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
                    command.CommandText = "update inventory set name=@name, code=@code, stock=@stock, sku=@sku, units=@units, gst=@gst, saleprice=@saleprice, purchaseprice=@purchaseprice, updated=@updated where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.ShortCode;
                    command.Parameters.Add("@stock", MySqlDbType.Double).Value = lead.Stock;
                    command.Parameters.Add("@sku", MySqlDbType.Double).Value = lead.SKU;
                    command.Parameters.Add("@units", MySqlDbType.VarChar).Value = lead.Units;
                    command.Parameters.Add("@gst", MySqlDbType.Double).Value = lead.GST;
                    command.Parameters.Add("@saleprice", MySqlDbType.Double).Value = lead.SellingPrice;
                    command.Parameters.Add("@purchaseprice", MySqlDbType.Double).Value = lead.PurchasePrice;
                    command.Parameters.Add("@updated", MySqlDbType.DateTime).Value = lead.Updated;
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

        public async Task DeleteInventoryAsync(InventoryModel lead)
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
                    command.CommandText = "delete from inventory where id=@id";
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
                            cmd.CommandText = $"SELECT name, code, stock, units FROM " + tableName + " limit 0";

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

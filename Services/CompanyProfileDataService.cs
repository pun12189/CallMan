using BahiKitab.Helper;
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
    public class CompanyProfileDataService : BaseDataService
    {
        public async Task<CompanyProfile> GetProfileAsync(int id)
        {
            CompanyProfile? lead = new();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from company_profile where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            lead = new CompanyProfile
                            {
                                Id = reader.GetInt32(0),
                                CompanyName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Proprietor = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Contact = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                GstNumber = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                BankName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                AccountNumber = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                IfscCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                UpiID = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Address = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                TermsAndConditions = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                Initials = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                InvNo = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                LogoPath = reader.IsDBNull(14) ? null : Helper.Helper.ByteArrayToImage((byte[])reader["logopath"]),
                                
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

        public async Task<ObservableCollection<CompanyProfile>> GetAllCompanyProfilesAsync()
        {
            // Simulates an asynchronous database call
            await Task.Delay(200);
            // REAL IMPLEMENTATION: Open connection, execute SELECT query, map results to CompanyProfile objects, close connection.

            // Return a clone of the collection for mock safety
            ObservableCollection<CompanyProfile> company_profile = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from company_profile";
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        company_profile = new ObservableCollection<CompanyProfile>();
                        while (reader.Read())
                        {
                            var lead = new CompanyProfile
                            {
                                Id = reader.GetInt32(0),
                                CompanyName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Proprietor = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Contact = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                GstNumber = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                BankName = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                AccountNumber = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                IfscCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                UpiID = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Address = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                TermsAndConditions = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                Initials = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                InvNo = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                LogoPath = reader.IsDBNull(14) ? null : Helper.Helper.ByteArrayToImage((byte[])reader["logopath"]),
                            };

                            company_profile.Add(lead);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //Helper.Helper.BugReport(e);
            }

            return company_profile;
        }

        public async Task<CompanyProfile> CreateCompanyProfileAsync(CompanyProfile lead)
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
                    command.CommandText = "insert into company_profile(company_name, proprietor, contact, email, gst, bank, account_no, ifsc, upi_id, address, terms, initials, invno, logopath) values(@company_name, @proprietor, @contact, @email, @gst, @bank, @account_no, @ifsc, @upi_id, @address, @terms, @initials, @invno, @logopath)";
                    command.Parameters.Add("@company_name", MySqlDbType.VarChar).Value = lead.CompanyName;
                    command.Parameters.Add("@proprietor", MySqlDbType.VarChar).Value = lead.Proprietor;
                    command.Parameters.Add("@contact", MySqlDbType.VarChar).Value = lead.Contact;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@gst", MySqlDbType.JSON).Value = lead.GstNumber;
                    command.Parameters.Add("@bank", MySqlDbType.JSON).Value = lead.BankName;
                    command.Parameters.Add("@account_no", MySqlDbType.JSON).Value = lead.AccountNumber;
                    command.Parameters.Add("@ifsc", MySqlDbType.JSON).Value = lead.IfscCode;
                    command.Parameters.Add("@upi_id", MySqlDbType.VarChar).Value = lead.UpiID;
                    command.Parameters.Add("@address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@terms", MySqlDbType.VarChar).Value = lead.TermsAndConditions;
                    command.Parameters.Add("@initials", MySqlDbType.VarChar).Value = lead.Initials;
                    command.Parameters.Add("@invno", MySqlDbType.Int32).Value = lead.InvNo;
                    command.Parameters.Add("@logopath", MySqlDbType.LongBlob).Value = Helper.Helper.ImageToByteArray(lead.LogoPath);
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

        public async Task<CompanyProfile> UpdateCompanyProfileAsync(CompanyProfile lead)
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
                    command.CommandText = "update company_profile set company_name=@company_name, proprietor=@proprietor, contact=@contact, email=@email, gst=@gst, bank=@bank, account_no=@account_no, ifsc=@ifsc, upi_id=@upi_id, address=@address, terms=@terms, initials=@initials, invno=@invno, logopath=@logopath where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@company_name", MySqlDbType.VarChar).Value = lead.CompanyName;
                    command.Parameters.Add("@proprietor", MySqlDbType.VarChar).Value = lead.Proprietor;
                    command.Parameters.Add("@contact", MySqlDbType.VarChar).Value = lead.Contact;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = lead.Email;
                    command.Parameters.Add("@gst", MySqlDbType.JSON).Value = lead.GstNumber;
                    command.Parameters.Add("@bank", MySqlDbType.JSON).Value = lead.BankName;
                    command.Parameters.Add("@account_no", MySqlDbType.JSON).Value = lead.AccountNumber;
                    command.Parameters.Add("@ifsc", MySqlDbType.JSON).Value = lead.IfscCode;
                    command.Parameters.Add("@upi_id", MySqlDbType.VarChar).Value = lead.UpiID;
                    command.Parameters.Add("@address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@terms", MySqlDbType.VarChar).Value = lead.TermsAndConditions;
                    command.Parameters.Add("@initials", MySqlDbType.VarChar).Value = lead.Initials;
                    command.Parameters.Add("@invno", MySqlDbType.Int32).Value = lead.InvNo;
                    command.Parameters.Add("@logopath", MySqlDbType.LongBlob).Value = Helper.Helper.ImageToByteArray(lead.LogoPath);
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

        public async Task DeleteCompanyProfileAsync(CompanyProfile lead)
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
                    command.CommandText = "delete from company_profile where id=@id";
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

using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BahiKitab.Helper
{
    public static class Helper
    {
        public static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }

        public static async Task BulkUpdateDataAsync(System.Data.DataTable dt)
        {
            var _connectionString = string.Empty;
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab;AllowLoadLocalInfile=true";
#endif
#if RELEASE

            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab;AllowLoadLocalInfile=true";
#endif
#if TESTING

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand("CREATE TABLE tmptable (`name` varchar(100), `code` varchar(100), `stocks` double, PRIMARY KEY (`code`))", conn))
                {
                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();

                        var bulkCopy = new MySqlBulkCopy(conn);
                        bulkCopy.DestinationTableName = "tmptable";
                        var result = await bulkCopy.WriteToServerAsync(dt);

                        command.CommandTimeout = 3000;
                        command.CommandText = "UPDATE actives INNER JOIN tmptable ON actives.code = tmptable.code SET actives.stocks = tmptable.stocks WHERE tmptable.code = actives.code OR tmptable.name = actives.name; DROP TABLE tmptable";
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bulk Update Failed: " + ex.Message, "Bulk Update", MessageBoxButton.OK, MessageBoxImage.Error);
                        using var cmd = new MySqlCommand("DROP TABLE tmptable;");
                        try
                        {
                            conn.Open();
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    finally
                    {
                        conn.Close();
                        MessageBox.Show("Bulk Update runs successfully. Please check the inventory by clicking on refresh button.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}

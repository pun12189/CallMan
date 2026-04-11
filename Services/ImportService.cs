using BahiKitab.Models;
using ClosedXML.Excel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Services
{
    public class ImportService : BaseDataService
    {
        public async Task ExecuteDynamicImport(string filePath, List<MappingRule> rules, int headerRowIndex, string tableName, List<string> excelHeaders)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var sheet = workbook.Worksheet(1);
                var dataRows = sheet.RowsUsed().Where(r => r.RowNumber() > headerRowIndex);

                using (var conn = GetConnection())
                {
                    await conn.OpenAsync();

                    foreach (var rule in rules.Where(r => !string.IsNullOrEmpty(r.ExcelHeader)))
                    {
                        await EnsureColumnExists(tableName, rule.DbColumnName);
                    }

                    foreach (var row in dataRows)
                    {
                        var parameters = new Dictionary<string, object>();

                        foreach (var rule in rules.Where(r => !string.IsNullOrEmpty(r.ExcelHeader)))
                        {
                            int colIndex = excelHeaders.IndexOf(rule.ExcelHeader) + 1;
                            var cellValue = row.Cell(colIndex).Value;

                            // Add to dictionary: Key = DB Column Name, Value = Excel Data
                            parameters.Add(rule.DbColumnName, cellValue.ToString());
                        }

                        if (parameters.Count > 0)
                        {
                            // Build dynamic SQL: INSERT INTO table (col1, col2) VALUES (@col1, @col2)
                            string columns = string.Join(", ", parameters.Keys);
                            string values = string.Join(", ", parameters.Keys.Select(k => "@" + k));
                            string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                            await conn.ExecuteAsync(sql, parameters);
                        }
                    }
                }
            }
        }

        private async Task EnsureColumnExists(string tableName, string columnName)
        {
            // Sanitize column name (remove spaces/special chars)
            string cleanColumnName = columnName.Replace(" ", "_").Trim();

            string checkSql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = @table 
                        AND COLUMN_NAME = @col 
                        AND TABLE_SCHEMA = DATABASE()";

            using (var conn = GetConnection())
            {
                int count = await conn.ExecuteScalarAsync<int>(checkSql, new { table = tableName, col = cleanColumnName });

                if (count == 0)
                {
                    // Add the column to MySQL dynamically
                    // We use TEXT as a safe default for unknown data
                    string alterSql = $"ALTER TABLE `{tableName}` ADD COLUMN `{cleanColumnName}` TEXT NULL";
                    await conn.ExecuteAsync(alterSql);
                }
            }                
        }
    }
}

using BahiKitab.Models;
using BahiKitab.Services;
using ClosedXML.Excel;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for GenericImportWindow.xaml
    /// </summary>
    public partial class GenericImportWindow : Window
    {
        public ObservableCollection<MappingRule> MappingRules { get; set; } = new();
        public List<string> ExcelHeaders { get; set; } = new();
        private string _tableName;
        private string _filePath;
        private int _headerRowIndex;

        // Pass the Type you want to map (e.g., typeof(Customer))
        public GenericImportWindow(string filePath, string tableName)
        {
            InitializeComponent();
            _filePath = filePath;
            _tableName = tableName;
            LoadExcelHeaders();
            GenerateMappingRows();

            dgMapping.ItemsSource = MappingRules;
            this.DataContext = this;
        }

        private void LoadExcelHeaders()
        {
            using (var workbook = new XLWorkbook(_filePath))
            {
                var sheet = workbook.Worksheet(1);
                // Scan first 10 rows to find a row that isn't empty (skipping company header)
                var headerRow = sheet.RowsUsed().FirstOrDefault(r => r.CellsUsed().Count() > 1);

                if (headerRow != null)
                {
                    _headerRowIndex = headerRow.RowNumber();
                    foreach (var cell in headerRow.CellsUsed())
                    {
                        ExcelHeaders.Add(cell.GetString());
                    }
                }
            }
        }

        private void GenerateMappingRows()
        {            
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            string connString = "Server=82.29.166.165;Port=3306;Uid=root;Pwd=sofricdev;database=bahikitabdev";
#endif
#if RELEASE

            //_connectionString = "Server=192.168.1.90;Uid=cosdb;Pwd=Cosmetify@123;database=cosmetify";
            string connString = "Server=82.29.166.165;Port=3307;Uid=root;Pwd=sofricprod;database=bahikitabprod";
#endif
#if TESTING

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            string connString = "Server=82.29.166.165;Port=3306;Uid=root;Pwd=sofricdev;database=bahikitabdev";
#endif
            using (var conn = new MySqlConnection(connString))
            {
                // Query to get all columns for the specific table
                string sql = @"SELECT COLUMN_NAME 
                           FROM INFORMATION_SCHEMA.COLUMNS 
                           WHERE TABLE_NAME = @tableName 
                           AND TABLE_SCHEMA = DATABASE()";

                var columns = conn.Query<string>(sql, new { tableName = _tableName });

                foreach (var col in columns)
                {
                    // Optionally skip auto-increment/primary key columns
                    if (col.ToLower() == "id") continue;

                    MappingRules.Add(new MappingRule { DbColumnName = col });
                }
            }
        }

        private void BtnRemoveRow_Click(object sender, RoutedEventArgs e)
        {
            var rule = (MappingRule)((Button)sender).DataContext;
            MappingRules.Remove(rule);
            UpdatePreview(); // Refresh preview after removing a field
        }

        private void BtnAddRow_Click(object sender, RoutedEventArgs e)
        {
            // Adds a blank mapping rule so the user can type a new DB column name
            MappingRules.Add(new MappingRule { DbColumnName = "New_Column_Name" });
        }

        private void CbExcelHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Give the UI a millisecond to update the binding
            Dispatcher.BeginInvoke(new Action(() => { UpdatePreview(); }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void UpdatePreview()
        {
            var previewTable = new DataTable();
            var activeRules = MappingRules.Where(r => !string.IsNullOrEmpty(r.ExcelHeader)).ToList();

            foreach (var rule in activeRules)
                previewTable.Columns.Add(rule.DbColumnName);

            // Load first 5 rows of data from our Excel source
            using (var workbook = new XLWorkbook(_filePath))
            {
                var sheet = workbook.Worksheet(1);
                var rows = sheet.RowsUsed().Skip(_headerRowIndex).Take(5);

                foreach (var row in rows)
                {
                    var newRow = previewTable.NewRow();
                    foreach (var rule in activeRules)
                    {
                        int colIndex = ExcelHeaders.IndexOf(rule.ExcelHeader) + 1;
                        newRow[rule.DbColumnName] = row.Cell(colIndex).Value.ToString();
                    }
                    previewTable.Rows.Add(newRow);
                }
            }
            dgPreview.ItemsSource = previewTable.DefaultView;
        }

        private async void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            // Filter rules where the user actually picked an Excel column
            var activeRules = MappingRules.Where(r => !string.IsNullOrEmpty(r.ExcelHeader)).ToList();

            if (!activeRules.Any())
            {
                MessageBox.Show("Please map at least one column.");
                return;
            }

            var service = new ImportService();

            // We pass 'ExcelHeaders' and '_headerRowIndex' which we found during the window load
            await service.ExecuteDynamicImport(
                _filePath,
                MappingRules.ToList(),
                _headerRowIndex,
                _tableName,
                ExcelHeaders
            );

            this.DialogResult = true;
        }
    }
}

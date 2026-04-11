using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BahiKitab.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        // Services
        private readonly InventoryDataService _dataService;

        public ICollectionView ProductView { get; set; }

        // Data Properties
        private ObservableCollection<InventoryModel> _leads;
        public ObservableCollection<InventoryModel> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        private InventoryModel _selectedLead;
        public InventoryModel SelectedLead
        {
            get => _selectedLead;
            set
            {
                if (Set(ref _selectedLead, value, nameof(SelectedLead)))
                {
                    // When selection changes, update the Edit/Delete UI state
                    UpdateCommandsCanExecute();
                    // If a lead is selected, clone it for editing
                    if (value != null)
                    {
                        // Simple clone for the edit form
                        CurrentLead = value.Clone();
                    }
                    else
                    {
                        CurrentLead = new InventoryModel(); // Clear form if deselected
                    }
                }
            }
        }

        private InventoryModel _currentLead = new InventoryModel();
        private string searchText;

        // This is the model used for the data entry form (Create/Update)
        public InventoryModel CurrentLead
        {
            get => _currentLead;
            set => Set(ref _currentLead, value, nameof(CurrentLead));
        }

        public string SearchText { get => searchText; set { 
                Set(ref searchText, value, nameof(SearchText)); 
                ProductView.Refresh();
            } 
        }

        // Commands
        public RelayCommand LoadLeadsCommand { get; private set; }
        public RelayCommand CreateUpdateLeadCommand { get; private set; }
        public RelayCommand UpdateLeadCommand { get; private set; }
        public RelayCommand DeleteLeadCommand { get; private set; }
        public RelayCommand NewLeadCommand { get; private set; }
        public RelayCommand UpdateInfoCommand { get; private set; }
        public RelayCommand ImportLeadsCommand { get; private set; }
        public RelayCommand BulkDeleteLeadCommand { get; private set; }

        public InventoryViewModel()
        {
            // Initialize service and data collections
            _dataService = new InventoryDataService();
            Leads = new ObservableCollection<InventoryModel>();
            ProductView = CollectionViewSource.GetDefaultView(Leads);
            ProductView.Filter = FilterProducts;

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            NewLeadCommand = new RelayCommand(_ => CreateNewLead());
            UpdateInfoCommand = new RelayCommand(_ => UpdateInfoLead());
            ImportLeadsCommand = new RelayCommand(async _ => await ImportLeadsCommandAsync());
            BulkDeleteLeadCommand = new RelayCommand(BulkDeleteLeadCommandAsync);

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private async void BulkDeleteLeadCommandAsync(object obj)
        {
            var grid = obj as DataGrid;
            if (grid?.SelectedItems.Count == 0) return;

            // Confirm with the user
            var result = MessageBox.Show($"Delete {grid.SelectedItems.Count} items?", "Confirm", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            // Convert SelectedItems to a list of IDs or SKUs
            var selectedList = grid.SelectedItems.Cast<InventoryModel>().ToList();
            var idsToDelete = selectedList.Select(p => p.Id).ToList(); // Using SKU as identifier

            // 1. Delete from MySQL
            bool success = await _dataService.BulkDeleteInventoryAsync(idsToDelete);

            if (success)
            {
                // 2. Remove from ObservableCollection to update UI instantly
                foreach (var item in selectedList)
                {
                    Leads.Remove(item);
                }
            }
        }

        private async Task ImportLeadsCommandAsync()
        {
            OpenFileDialog openFile = new OpenFileDialog { Filter = "Excel Files | *.xlsx" };
            if (openFile.ShowDialog() == true)
            {
                // 1. Open the Mapping UI
                var mapWin = new GenericImportWindow(openFile.FileName, "inventory");

                if (mapWin.ShowDialog() == true)
                {                    
                    MessageBox.Show("Import Successful!");
                }
            }

            //var view = new ImportDialog();
            //view.DataContext = this;
            //var window = new Window();
            //window.Title = "Import Inventory";
            //window.Content = view;
            //window.Width = 600;
            //window.SizeToContent = SizeToContent.Height;
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //var res = window.ShowDialog();
            //if (res is true)
            //{
            //    MessageBox.Show("Inventory will be imported at background when implemented.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //}

            // Create OpenFileDialog 
            /*Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Excel files(*.csv, *.xls, *.xlsx) | *.csv; *.xls; *.xlsx | All files(*.*) | *.* ";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                var dataTable = Helper.Helper.ConvertCsvToDataTable(dlg.FileName);
                await _dataService.BulkInsertMySQL(dataTable, "inventory");
                await LoadLeadsAsync();
            }*/
        }

        private async Task UpdateInfoLead()
        {
            var view = new LeadStatusView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Inventory";
            window.Content = view;
            window.Width = 600;
            window.SizeToContent = SizeToContent.Height;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var res = window.ShowDialog();
            if (res is true)
            {
                CurrentLead = this.SelectedLead;
                await SaveLeadAsync();
            }
        }

        private void UpdateCommandsCanExecute()
        {
            // Manually trigger RequerySuggested for relevant commands
            CreateUpdateLeadCommand.RaiseCanExecuteChanged();
            DeleteLeadCommand.RaiseCanExecuteChanged();
        }

        private bool CanSaveLead()
        {
            // Basic validation: must have First Name and Email
            return !string.IsNullOrEmpty(CurrentLead?.Name) && CurrentLead?.Stock != 0.0;
        }

        private async Task LoadLeadsAsync()
        {            
            try
            {
                var data = await _dataService.GetAllInventoryAsync();

                // Clear and update the collection on the UI Thread
                Leads.Clear();
                foreach (var item in data)
                {
                    Leads.Add(item);
                }

                // Force the View to refresh to reflect the new data
                ProductView.Refresh();
            }
            catch (Exception ex)
            {
                // Handle DB connection errors here
                MessageBox.Show($"Error loading inventory: {ex.Message}");
            }
        }

        private bool FilterProducts(object obj)
        {
            if (obj is InventoryModel product)
            {
                return string.IsNullOrWhiteSpace(SearchText) ||
                       product.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       product.ShortCode.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void CreateNewLead()
        {
            // Clear selection and form for a new entry
            SelectedLead = null;
            CurrentLead = new InventoryModel();
            var view = new AddInventoryView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Add Inventory";
            window.Content = view;
            window.Width = 600;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void UpdateNewLead()
        {
            // Clear selection and form for a new entry
            CurrentLead = this.SelectedLead;
            var view = new AddInventoryView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Inventory";
            window.Content = view;
            window.Width = 600;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private async Task SaveLeadAsync()
        {
            if (CurrentLead.Id == 0)
            {
                // CREATE NEW LEAD
                var createdLead = await _dataService.CreateInventoryAsync(CurrentLead);
                Leads.Add(createdLead);
                MessageBox.Show($"Inventory {createdLead.Name} created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // UPDATE EXISTING LEAD
                await _dataService.UpdateInventoryAsync(CurrentLead);

                // Find the original object in the collection and update its properties
                var existingLead = Leads.FirstOrDefault(l => l.Id == CurrentLead.Id);
                if (existingLead != null)
                {
                    existingLead = CurrentLead.Clone();
                }
                MessageBox.Show($"Inventory {CurrentLead.Id} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteLeadAsync()
        {
            if (SelectedLead == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete Inventory: {SelectedLead.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _dataService.DeleteInventoryAsync(SelectedLead);
                // The data service updates the mock list, so we just need to refresh the bound collection.
                Leads.Remove(SelectedLead);
                SelectedLead = null; // Clear selection after deletion
                MessageBox.Show("Inventory deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.ViewModels
{
    public class MatureLeadsViewModel : ViewModelBase
    {
        // Services
        private readonly LeadsDataService _dataService;

        // Data Properties
        private ObservableCollection<Lead> _leads;
        public ObservableCollection<Lead> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        private Lead _selectedLead;
        public Lead SelectedLead
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
                        CurrentLead = new Lead(); // Clear form if deselected
                    }
                }
            }
        }

        private Lead _currentLead = new Lead();
        // This is the model used for the data entry form (Create/Update)
        public Lead CurrentLead
        {
            get => _currentLead;
            set => Set(ref _currentLead, value, nameof(CurrentLead));
        }

        // Commands
        public RelayCommand LoadLeadsCommand { get; private set; }
        public RelayCommand CreateUpdateLeadCommand { get; private set; }
        public RelayCommand UpdateLeadCommand { get; private set; }
        public RelayCommand DeleteLeadCommand { get; private set; }
        public RelayCommand NewLeadCommand { get; private set; }
        public RelayCommand UpdateInfoCommand { get; private set; }
        public RelayCommand ImportLeadsCommand { get; private set; }

        public MatureLeadsViewModel()
        {
            // Initialize service and data collections
            _dataService = new LeadsDataService();
            Leads = new ObservableCollection<Lead>();

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            NewLeadCommand = new RelayCommand(_ => CreateNewLead());
            UpdateInfoCommand = new RelayCommand(_ => UpdateInfoLead());
            ImportLeadsCommand = new RelayCommand(async _ => await ImportLeadsCommandAsync());

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private async Task ImportLeadsCommandAsync()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Excel files(*.csv, *.xls, *.xlsx) | *.csv; *.xls; *.xlsx | All files(*.*) | *.* ";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // var dataTable = Helper.Helper.UpdateConvertCsvToDataTable(dlg.FileName);
                // await Helper.Helper.BulkUpdateDataAsync(dataTable);
            }
        }

        private async Task UpdateInfoLead()
        {
            var view = new LeadStatusView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Lead";
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
            return !string.IsNullOrEmpty(CurrentLead?.Name) && !string.IsNullOrEmpty(CurrentLead?.Phone);
        }

        private async Task LoadLeadsAsync()
        {
            Leads = await _dataService.GetAllLeadsAsync();
        }

        private void CreateNewLead()
        {
            // Clear selection and form for a new entry
            SelectedLead = null;
            CurrentLead = new Lead();
            var view = new AddLeadsView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Add Lead";
            window.Content = view;
            window.Width = 600;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private void UpdateNewLead()
        {
            // Clear selection and form for a new entry
            CurrentLead = this.SelectedLead;
            var view = new AddLeadsView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Lead";
            window.Content = view;
            window.Width = 600;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private async Task SaveLeadAsync()
        {
            if (CurrentLead.Id == 0)
            {
                // CREATE NEW LEAD
                Lead createdLead = await _dataService.CreateLeadAsync(CurrentLead);
                Leads.Add(createdLead);
                MessageBox.Show($"Lead {createdLead.Name} created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // UPDATE EXISTING LEAD
                await _dataService.UpdateLeadAsync(CurrentLead);

                // Find the original object in the collection and update its properties
                var existingLead = Leads.FirstOrDefault(l => l.Id == CurrentLead.Id);
                if (existingLead != null)
                {
                    existingLead.Name = CurrentLead.Name;
                    existingLead.Company = CurrentLead.Company;
                    existingLead.Email = CurrentLead.Email;
                    existingLead.Phone = CurrentLead.Phone;
                    existingLead.Status = CurrentLead.Status;
                    existingLead.LeadSource = CurrentLead.LeadSource;
                    existingLead.Tags = CurrentLead.Tags;
                    existingLead.Label = CurrentLead.Label;
                    existingLead.City = CurrentLead.City;
                    existingLead.State = CurrentLead.State;
                    existingLead.District = CurrentLead.District;
                    existingLead.Pincode = CurrentLead.Pincode;
                    existingLead.Country = CurrentLead.Country;
                    existingLead.ImpDate = CurrentLead.ImpDate;
                    existingLead.AltPhone = CurrentLead.AltPhone;
                    existingLead.FirmName = CurrentLead.FirmName;
                    existingLead.UpdationDate = CurrentLead.UpdationDate;
                    existingLead.LeadType = CurrentLead.LeadType;
                }
                MessageBox.Show($"Lead {CurrentLead.Id} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteLeadAsync()
        {
            if (SelectedLead == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete lead: {SelectedLead.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _dataService.DeleteLeadAsync(SelectedLead);
                // The data service updates the mock list, so we just need to refresh the bound collection.
                Leads.Remove(SelectedLead);
                SelectedLead = null; // Clear selection after deletion
                MessageBox.Show("Lead deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

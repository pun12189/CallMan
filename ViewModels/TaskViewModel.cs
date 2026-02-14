using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitab.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private readonly DepartmentsDataService _dataService;
        private readonly StaffDataService _staffDataService;
        private readonly TaskDataService taskDataService;
        // Data Properties
        private ObservableCollection<TaskModel> _leads;
        public ObservableCollection<TaskModel> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        private TaskModel _selectedLead;
        public TaskModel SelectedLead
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
                        CurrentLead = new TaskModel(); // Clear form if deselected
                    }
                }
            }
        }

        private TaskModel _currentLead = new TaskModel();
        private ObservableCollection<StaffModel> allStaff;
        private ObservableCollection<DepartmentsModel> allDepartments;

        // This is the model used for the data entry form (Create/Update)
        public TaskModel CurrentLead
        {
            get => _currentLead;
            set => Set(ref _currentLead, value, nameof(CurrentLead));
        }

        public ObservableCollection<StaffModel> AllStaff { get => allStaff; set => Set(ref allStaff, value, nameof(AllStaff)); }
        public ObservableCollection<DepartmentsModel> AllDepartments { get => allDepartments; set => Set(ref allDepartments, value, nameof(AllDepartments)); }

        // Commands
        public RelayCommand LoadLeadsCommand { get; private set; }
        public RelayCommand CreateUpdateLeadCommand { get; private set; }
        public RelayCommand UpdateLeadCommand { get; private set; }
        public RelayCommand DeleteLeadCommand { get; private set; }
        public RelayCommand WhatsappCommand { get; private set; }

        public TaskViewModel()
        {
            _dataService = new DepartmentsDataService();
            _staffDataService = new StaffDataService();
            taskDataService = new TaskDataService();

            Leads = new ObservableCollection<TaskModel>();

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            WhatsappCommand = new RelayCommand(WhatsappCommandExecute);

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private void WhatsappCommandExecute(object obj)
        {
            var model = obj as Lead;

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Phone))
                {
                    // Phone number se extra characters (+, spaces, dashes) hatane ke liye
                    string cleanNumber = new string(model.Phone.Where(char.IsDigit).ToArray());

                    // Agar number 10 digit ka hai, toh country code (e.g., 91) add karna zaroori hai
                    if (cleanNumber.Length == 10)
                    {
                        cleanNumber = "91" + cleanNumber;
                    }

                    string message = $"Hello {model.Name} , \n\n" +
                         $"Thanks for visiting our store \n" +
                         $"Please feel free to contact us on this whatsapp \n" +
                         $"_automated msg, sent from SofricERP_";

                    string encodedMessage = Uri.EscapeDataString(message);

                    // WhatsApp Web URL
                    string url = $"https://web.whatsapp.com/send?phone={cleanNumber}&text={encodedMessage}";

                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        // Error handling agar browser open na ho sake
                        Debug.WriteLine(ex.Message);
                    }
                }
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
            return !string.IsNullOrEmpty(CurrentLead?.OrderId);
        }

        private async Task LoadLeadsAsync()
        {
            Leads = await taskDataService.GetAllLeadHistorysAsync();
            AllDepartments = await _dataService.GetAllDepartmentsAsync();
            AllStaff = await _staffDataService.GetAllStaffAsync();
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
                TaskModel createdLead = await taskDataService.CreateLeadHistoryAsync(CurrentLead);
                Leads.Add(createdLead);
                MessageBox.Show($"Task {createdLead.Id} created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // UPDATE EXISTING LEAD
                await taskDataService.UpdateLeadHistoryAsync(CurrentLead);

                // Find the original object in the collection and update its properties
                var existingLead = Leads.FirstOrDefault(l => l.Id == CurrentLead.Id);
                if (existingLead != null)
                {
                    existingLead = CurrentLead.Clone();
                }

                MessageBox.Show($"Task {CurrentLead.Id} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteLeadAsync()
        {
            if (SelectedLead == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete Task: {SelectedLead.Id}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await taskDataService.DeleteLeadHistoryAsync(SelectedLead);
                // The data service updates the mock list, so we just need to refresh the bound collection.
                Leads.Remove(SelectedLead);
                SelectedLead = null; // Clear selection after deletion
                MessageBox.Show("Task deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

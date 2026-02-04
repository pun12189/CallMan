using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.ViewModels;
using BahiKitab.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BahiKitab.ViewModels
{
    public class StaffViewModel : ViewModelBase
    {
        // Services
        private readonly StaffDataService _dataService;

        // Data Properties
        private ObservableCollection<StaffModel> _staffTL;
        public ObservableCollection<StaffModel> StaffTL
        {
            get => _staffTL;
            set => Set(ref _staffTL, value, nameof(StaffTL));
        }

        // Data Properties
        private ObservableCollection<StaffModel> _staff;
        public ObservableCollection<StaffModel> Staff
        {
            get => _staff;
            set => Set(ref _staff, value, nameof(Staff));
        }

        private StaffModel _selectedLead;
        public StaffModel SelectedLead
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
                        CurrentLead = new StaffModel(); // Clear form if deselected
                    }
                }
            }
        }

        private StaffModel _currentLead = new StaffModel();
        // This is the model used for the data entry form (Create/Update)
        public StaffModel CurrentLead
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
        public RelayCommand UploadProfileCommand { get; private set; }
        public RelayCommand ImportLeadsCommand { get; private set; }

        public StaffViewModel()
        {
            // Initialize service and data collections
            _dataService = new StaffDataService();
            Staff = new ObservableCollection<StaffModel>();
            StaffTL = new ObservableCollection<StaffModel>();

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            NewLeadCommand = new RelayCommand(_ => CreateNewLead());
            UpdateInfoCommand = new RelayCommand(_ => UpdateInfoLead());
            UploadProfileCommand = new RelayCommand(_ => UploadProfileImage());

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private void UploadProfileImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Create a bitmap from the selected file
                Uri fileUri = new Uri(openFileDialog.FileName);
                BitmapImage bitmap = new BitmapImage(fileUri);

                // Set the ImageBrush source to the new image
                CurrentLead.ProfileImage = bitmap;
            }
        }

        private async Task UpdateInfoLead()
        {
            var view = new LeadStatusView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Staff";
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
            return !string.IsNullOrEmpty(CurrentLead?.FullName) && !string.IsNullOrEmpty(CurrentLead?.Phone);
        }

        private async Task LoadLeadsAsync()
        {
            Staff = await _dataService.GetAllStaffAsync();
            StaffTL = await _dataService.GetAllStaffAsync();
        }

        private void CreateNewLead()
        {
            // Clear selection and form for a new entry
            SelectedLead = null;
            CurrentLead = new StaffModel();
            var view = new AddStaffProfile();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Add Staff";
            window.Content = view;
            window.Width = 600;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private void UpdateNewLead()
        {
            // Clear selection and form for a new entry
            CurrentLead = this.SelectedLead;
            var view = new AddStaffProfile();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Staff";
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
                StaffModel createdLead = await _dataService.CreateStaffAsync(CurrentLead);
                if (Staff == null)
                {
                    Staff = new ObservableCollection<StaffModel>();
                }

                Staff.Add(createdLead);
                MessageBox.Show($"Staff {createdLead.FullName} created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // UPDATE EXISTING LEAD
                await _dataService.UpdateStaffAsync(CurrentLead);

                // Find the original object in the collection and update its properties
                var existingLead = Staff.FirstOrDefault(l => l.Id == CurrentLead.Id);
                if (existingLead != null)
                {
                    existingLead.FullName = CurrentLead.FullName;
                    existingLead.Email = CurrentLead.Email;
                    existingLead.Phone = CurrentLead.Phone;
                    existingLead.IsActive = CurrentLead.IsActive;
                    existingLead.Department = CurrentLead.Department;
                    existingLead.TeamLead = CurrentLead.TeamLead;
                    existingLead.Username = CurrentLead.Username;
                    existingLead.Password = CurrentLead.Password;
                    existingLead.Address = CurrentLead.Address;
                    existingLead.Role = CurrentLead.Role;
                    existingLead.ProfileImage = CurrentLead.ProfileImage;
                    existingLead.UpdateTime = CurrentLead.UpdateTime;
                    existingLead.CreateTime = CurrentLead.CreateTime;
                }

                MessageBox.Show($"Staff {CurrentLead.FullName} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteLeadAsync()
        {
            if (SelectedLead == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete staff: {SelectedLead.FullName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _dataService.DeleteStaffAsync(SelectedLead);
                // The data service updates the mock list, so we just need to refresh the bound collection.
                Staff.Remove(SelectedLead);
                SelectedLead = null; // Clear selection after deletion
                MessageBox.Show("Staff deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

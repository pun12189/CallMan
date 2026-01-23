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
    public class OrderViewModel : ViewModelBase
    {
        // Services
        private readonly LeadsOrderDataService _dataService;

        // Data Properties
        private ObservableCollection<LeadOrderModel> _leads;
        public ObservableCollection<LeadOrderModel> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        private LeadOrderModel _selectedLead;
        public LeadOrderModel SelectedLead
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
                        CurrentLead = new LeadOrderModel(); // Clear form if deselected
                    }
                }
            }
        }

        private LeadOrderModel _currentLead = new LeadOrderModel();
        // This is the model used for the data entry form (Create/Update)
        public LeadOrderModel CurrentLead
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

        public OrderViewModel()
        {
            // Initialize service and data collections
            _dataService = new LeadsOrderDataService();
            Leads = new ObservableCollection<LeadOrderModel>();

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            NewLeadCommand = new RelayCommand(_ => CreateNewLead());
            UpdateInfoCommand = new RelayCommand(_ => UpdateInfoLead());            

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private async Task UpdateInfoLead()
        {
            var view = new LeadStatusView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Order";
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
            return CurrentLead?.OrderAmount != 0.0 && CurrentLead?.ReceivedAmount != 0.0;
        }

        private async Task LoadLeadsAsync()
        {
            Leads = await _dataService.GetAllOrdersAsync();
        }

        private void CreateNewLead()
        {
            // Clear selection and form for a new entry
            SelectedLead = null;
            CurrentLead = new LeadOrderModel();
            var view = new AddLeadsView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Add Order";
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
            window.Title = "Update Order";
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
                LeadOrderModel createdLead = await _dataService.CreateLeadOrderAsync(CurrentLead);
                Leads.Add(createdLead);
                MessageBox.Show($"Order {createdLead.OrderId} of Customer {createdLead.Customer?.Name} created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // UPDATE EXISTING LEAD
                await _dataService.UpdateLeadOrderAsync(CurrentLead);

                // Find the original object in the collection and update its properties
                var existingLead = Leads.FirstOrDefault(l => l.Id == CurrentLead.Id);
                if (existingLead != null)
                {
                    existingLead.OrderAmount = CurrentLead.OrderAmount;
                    existingLead.ReceivedAmount = CurrentLead.ReceivedAmount;
                    existingLead.LastMsg = CurrentLead.LastMsg;
                    existingLead.Updated = CurrentLead.Updated;
                    existingLead.OrderStatus = CurrentLead.OrderStatus;
                    existingLead.PaymentStatus = CurrentLead.PaymentStatus;
                    existingLead.PaymentType = CurrentLead.PaymentType;
                    existingLead.IsAccepted = CurrentLead.IsAccepted;
                    existingLead.Customer = CurrentLead.Customer.Clone();
                    existingLead.AcceptedDate = CurrentLead.AcceptedDate;
                    existingLead.Discount = CurrentLead.Discount;
                    existingLead.TakenBy = CurrentLead.TakenBy;
                    existingLead.Balance = CurrentLead.Balance;
                    existingLead.NextFollowup = CurrentLead.NextFollowup;
                }
                MessageBox.Show($"Order {CurrentLead.OrderId} updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteLeadAsync()
        {
            if (SelectedLead == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete order: {SelectedLead.OrderId}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _dataService.DeleteLeadOrderAsync(SelectedLead);
                // The data service updates the mock list, so we just need to refresh the bound collection.
                Leads.Remove(SelectedLead);
                SelectedLead = null; // Clear selection after deletion
                MessageBox.Show("Order deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

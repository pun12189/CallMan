using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        private readonly ImagesDataService imagesDataService;
        private readonly LeadsDataService leadsDataService;
        private readonly TaskDataService tasksDataService;
        private readonly CompanyProfileDataService companyProfileDataService;

        // Data Properties
        private ObservableCollection<LeadOrderModel> _leads;
        public ObservableCollection<LeadOrderModel> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        private CompanyProfile profile;
        public CompanyProfile Profile { get => profile; set => Set(ref profile, value, nameof(Profile)); }

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
        public RelayCommand ImportOrdersCommand { get; private set; }
        public RelayCommand ReferenceImagesCommand { get; private set; }

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand WhatsappCommand { get; private set; }

        public RelayCommand PdfCommand { get; private set; }

        public RelayCommand TaskCreateCommand { get; private set; }


        public OrderViewModel()
        {
            // Initialize service and data collections
            _dataService = new LeadsOrderDataService();
            imagesDataService = new ImagesDataService();
            leadsDataService = new LeadsDataService();
            tasksDataService = new TaskDataService();
            companyProfileDataService = new CompanyProfileDataService();
            Leads = new ObservableCollection<LeadOrderModel>();

            // Initialize Commands
            LoadLeadsCommand = new RelayCommand(async _ => await LoadLeadsAsync());
            CreateUpdateLeadCommand = new RelayCommand(async _ => await SaveLeadAsync(), _ => CanSaveLead());
            UpdateLeadCommand = new RelayCommand(_ => UpdateNewLead());
            DeleteLeadCommand = new RelayCommand(async _ => await DeleteLeadAsync(), _ => SelectedLead != null);
            NewLeadCommand = new RelayCommand(_ => CreateNewLead());
            UpdateInfoCommand = new RelayCommand(_ => UpdateInfoLead());
            ImportOrdersCommand = new RelayCommand(_ => ImportOrders());
            ReferenceImagesCommand = new RelayCommand(_ => AddRefImages());
            EditCommand = new RelayCommand(EditCommandExecute);
            SaveCommand = new RelayCommand(SaveCommandExecute);
            WhatsappCommand = new RelayCommand(WhatsappCommandExecute);
            PdfCommand = new RelayCommand(PdfCommandExecute);
            TaskCreateCommand = new RelayCommand(TaskCreateCommandExecute);

            // Load data immediately upon initialization
            LoadLeadsCommand.Execute(null);
        }

        private async void TaskCreateCommandExecute(object obj)
        {
            var model = obj as LeadOrderModel;

            if (model != null)
            {
                model.IsTaskCreated = true;
                if (model.OrderedProducts != null)
                {
                    foreach (var item in model.OrderedProducts)
                    {
                        var task = new TaskModel();
                        task.Product = item.Clone();
                        task.Customer = model.Customer.Clone();
                        task.OrderId = model.OrderId;

                        await tasksDataService.CreateLeadHistoryAsync(task);
                    }
                }
                
                await _dataService.UpdateLeadOrderAsync(model);
            }
        }

        private async void PdfCommandExecute(object obj)
        {
            var pdf = new PdfForm();
            var model = obj as LeadOrderModel;           

            if (model != null)
            {
                pdf.CreateFreePdf(model, profile);
            }
        }

        private void WhatsappCommandExecute(object obj)
        {
            var model = obj as LeadOrderModel;

            if (model != null) 
            {
                if (!string.IsNullOrEmpty(model.Customer.Phone))
                {
                    // Phone number se extra characters (+, spaces, dashes) hatane ke liye
                    string cleanNumber = new string(model.Customer.Phone.Where(char.IsDigit).ToArray());

                    // Agar number 10 digit ka hai, toh country code (e.g., 91) add karna zaroori hai
                    if (cleanNumber.Length == 10)
                    {
                        cleanNumber = "91" + cleanNumber;
                    }

                    string message = $"Hello {model.Customer.Name} , \n\n" +
                         $"Your bill number {model.OrderId} is generated। \n" +
                         $"Total Amount: ₹{model.OrderAmount} \n" +
                         $"Received Amount: ₹{model.ReceivedAmount} \n\n" +
                         $"Balance Amount: ₹{model.Balance} \n\n" +
                         $"Thanks \n" +
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

        private async void SaveCommandExecute(object obj)
        {
            this.CurrentLead = this.SelectedLead;
            await SaveLeadAsync();
            var model = obj as LeadOrderModel;
            if (model != null)
            {
                model.IsEditing = false;
            }
        }

        private void EditCommandExecute(object obj)
        {
            var model = obj as LeadOrderModel;
            if (model != null) 
            {
                model.IsEditing = true;
            }
        }

        private async void AddRefImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
                    string fileName = System.IO.Path.GetFileName(filePath);
                    CurrentLead.ReferenceImages.Add(await imagesDataService.CreateImageAsync(fileName, imageBytes));
                }                
            }
        }

        private void ImportOrders()
        {
            OpenFileDialog openFile = new OpenFileDialog { Filter = "Excel Files|*.xlsx" };
            if (openFile.ShowDialog() == true)
            {
                // 1. Open the Mapping UI
                var mapWin = new GenericImportWindow(openFile.FileName, "lead_orders");

                if (mapWin.ShowDialog() == true)
                {
                    MessageBox.Show("Import Successful!");
                }
            }

            /*var view = new ImportDialog();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Import Order";
            window.Content = view;
            window.Width = 600;
            window.SizeToContent = SizeToContent.Height;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var res = window.ShowDialog();
            if (res is true)
            {
                MessageBox.Show("Orders will be imported at background when implemented.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }*/
        }

        private async Task UpdateInfoLead()
        {
            var view = new MatureLeadStatusView();
            view.DataContext = this;
            var window = new Window();
            window.Title = "Update Order Status";
            window.Content = view;
            window.Width = 600;
            window.SizeToContent = SizeToContent.Height;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var res = window.ShowDialog();
            if (res is true)
            {
                this.SelectedLead.Updated = DateTime.Now;
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
            return CurrentLead?.OrderAmount != 0.0;
        }

        private async Task LoadLeadsAsync()
        {
            Leads = await _dataService.GetAllOrdersAsync();
            Profile = await companyProfileDataService.GetProfileAsync(1);
        }

        private void CreateNewLead()
        {
            // Clear selection and form for a new entry
            SelectedLead = null;
            CurrentLead = new LeadOrderModel();
            var view = new CreateOrderView();
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
            var view = new CreateOrderView();
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
                CurrentLead.Balance = CurrentLead.OrderAmount - CurrentLead.ReceivedAmount;

                if (CurrentLead.Balance <= 0.0)
                {
                    CurrentLead.PaymentStatus = Helper.PaymentStatus.Paid;
                    CurrentLead.PaymentType = Helper.PaymentType.Cash;
                }
                else if (CurrentLead.Balance == CurrentLead.OrderAmount)
                {
                    CurrentLead.PaymentStatus = Helper.PaymentStatus.Unpaid;
                    CurrentLead.PaymentType = Helper.PaymentType.Credit;
                }
                else
                {
                    CurrentLead.PaymentStatus = Helper.PaymentStatus.PartialPaid;
                    CurrentLead.PaymentType = Helper.PaymentType.Credit;
                }

                CurrentLead.OrderId = Profile.Initials + DateTime.Now.GetHashCode();
                CurrentLead.IsAccepted = false;
                CurrentLead.Customer.LeadType = Helper.LeadType.Matured;

                LeadOrderModel createdLead = await _dataService.CreateLeadOrderAsync(CurrentLead);

                await leadsDataService.UpdateLeadAsync(CurrentLead.Customer);

                Leads.Add(createdLead);
                App.BackgroundWorker.EnqueueWork("OnNewOrder", createdLead.Customer);
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

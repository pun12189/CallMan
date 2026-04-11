using BahiKitab.Core;
using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BahiKitab.ViewModels
{
    public class PurchaseViewModel : ViewModelBase
    {
        private readonly PurchaseDataService _repo = new();
        private readonly VendorDataService _vrepo = new();
        private readonly InventoryDataService _irepo = new();
        private readonly string MyStateCode = "03"; // Example: Punjab State Code

        // --- Collections ---
        public ObservableCollection<VendorModel> Vendors { get; set; } = new();
        public ObservableCollection<PurchaseItem> PurchaseItems { get; set; } = new();

        public VendorModel NewVendor { get; set; } = new();

        // --- Header Properties ---
        private VendorModel _selectedVendor;
        public VendorModel SelectedVendor
        {
            get => _selectedVendor;
            set
            {
                Set(ref _selectedVendor, value, nameof(SelectedVendor));
                OnPropertyChanged(nameof(IsInterState)); // Refresh tax logic
                CalculateTotals();
            }
        }

        private string _invoiceNo;
        public string InvoiceNo { get => _invoiceNo; set => Set(ref _invoiceNo, value, nameof(InvoiceNo)); }

        private string _originalInvNo;
        public string OriginalInvNo { get => _originalInvNo; set => Set(ref _originalInvNo, value, nameof(OriginalInvNo)); }

        private DateTime _invoiceDate = DateTime.Now;
        public DateTime InvoiceDate { get => _invoiceDate; set => Set(ref _invoiceDate, value, nameof(InvoiceDate)); }

        // --- Footer Summary Properties ---
        private double _totalTaxable;
        public double TotalTaxable { get => _totalTaxable; set => Set(ref _totalTaxable, value, nameof(TotalTaxable)); }

        private double _totalCGST;
        public double TotalCGST { get => _totalCGST; set => Set(ref _totalCGST, value, nameof(TotalCGST)); }

        private double _totalSGST;
        public double TotalSGST { get => _totalSGST; set => Set(ref _totalSGST, value, nameof(TotalSGST)); }

        private double _totalIGST;
        public double TotalIGST { get => _totalIGST; set => Set(ref _totalIGST, value, nameof(TotalIGST)); }

        private double _grandTotal;
        public double GrandTotal { get => _grandTotal; set => Set(ref _grandTotal, value, nameof(GrandTotal)); }

        // --- Logic Properties ---
        public bool IsInterState => SelectedVendor != null &&
                                    !string.IsNullOrEmpty(SelectedVendor.GSTIN) &&
                                    SelectedVendor.GSTIN.Substring(0, 2) != MyStateCode;

        public ObservableCollection<InventoryModel> AllProducts { get; set; } = new();
        public List<InventoryModel> FilteredProducts => AllProducts
            .Where(p => string.IsNullOrEmpty(SearchText) || p.Name.ToLower().Contains(SearchText.ToLower()))
            .ToList();

        private string _searchText;
        public string SearchText { get => _searchText; set { Set(ref _searchText, value, nameof(SearchText)); OnPropertyChanged(nameof(FilteredProducts)); } }

        public InventoryModel SelectedProduct { get; set; }

        // UI States
        private bool _isAddFormVisible;
        public bool IsAddFormVisible { get => _isAddFormVisible; set { Set(ref _isAddFormVisible, value, nameof(IsAddFormVisible)); } }

        // New Product Fields
        public string NewProductName { get; set; }
        public string NewHSN { get; set; }
        public string NewCode { get; set; }

        // Commands
        public ICommand ToggleAddFormCommand { get; set; }
        public ICommand SaveNewProductCommand { get; set; }

        public ICommand SelectProductCommand { get; set; }

        // --- Commands ---
        public ICommand SaveCommand { get; set; }
        public ICommand AddItemCommand { get; set; }
        public ICommand AddVendorCommand { get; set; }
        public ICommand SaveVendorCommand { get; set; }
        public event Action OnSuccess;
        public event Action<InventoryModel> OnProductChosen;

        public PurchaseViewModel()
        {
            SaveCommand = new RelayCommand(async _ => await ExecuteSave());
            AddItemCommand = new RelayCommand(_ => ExecuteAddItem());
            AddVendorCommand = new RelayCommand(_ => OpenAddVendorDialog());
            SaveVendorCommand = new RelayCommand(async _ => await SaveVendor());

            // Listen for list changes to recalculate footer
            PurchaseItems.CollectionChanged += (s, e) => CalculateTotals();

            SelectProductCommand = new RelayCommand(_ => {
                if (SelectedProduct != null) OnProductChosen?.Invoke(SelectedProduct);
            });

            ToggleAddFormCommand = new RelayCommand(_ => IsAddFormVisible = !IsAddFormVisible);

            SaveNewProductCommand = new RelayCommand(async _ => await ExecuteQuickAdd());

            LoadProducts();

            LoadInitialData();
        }

        private void OpenAddVendorDialog()
        {            
            var dialog = new AddVendorDialog { DataContext = this };

            this.OnSuccess += async () => {
                dialog.Close();
                await LoadInitialData(); // Reload the list from MySQL
            };

            dialog.ShowDialog();
        }

        private async Task SaveVendor()
        {
            if (string.IsNullOrEmpty(NewVendor.Name))
            {
                MessageBox.Show("Name is required!");
                return;
            }
            
            var id = await _vrepo.CreateVendorAsync(NewVendor);

            if (id != 0)
            {
                OnSuccess?.Invoke();
            }
        }

        private async Task LoadInitialData()
        {
            // Load Vendors from DB via Repository
            var list = await _vrepo.GetAllVendorsAsync();
            if (list != null)
            {
                Vendors.Clear();
                foreach (var v in list) Vendors.Add(v);
            }            
        }

        private async void LoadProducts()
        {
            var list = await _irepo.GetAllInventoryAsync();
            if (list != null)
            {
                foreach (var p in list) AllProducts.Add(p);
            }
        }

        private async Task ExecuteQuickAdd()
        {
            if (string.IsNullOrWhiteSpace(NewProductName)) return;

            var newProd = new InventoryModel
            {
                Name = NewProductName,
                HSN = NewHSN,
                BasePrice = 0,
                ShortCode = NewCode // User will fill price in the main purchase grid
            };

            // 1. Save to Database
            await _irepo.CreateInventoryAsync(newProd);

            // 2. Automatically select this product and close dialog
            OnProductChosen?.Invoke(newProd);
        }

        public void CalculateTotals()
        {
            var taxable = PurchaseItems.Sum(x => x.TaxableAmount);
            var totalTax = PurchaseItems.Sum(x => x.TaxValue);

            TotalTaxable = taxable;

            if (IsInterState)
            {
                TotalIGST = totalTax;
                TotalCGST = 0;
                TotalSGST = 0;
            }
            else
            {
                TotalIGST = 0;
                TotalCGST = totalTax / 2;
                TotalSGST = totalTax / 2;
            }

            GrandTotal = Math.Round(TotalTaxable + totalTax, 2);

            // Notify UI of Visibility changes for Tax Labels
            OnPropertyChanged(nameof(IsInterState));
        }

        private void ExecuteAddItem()
        {            
            var dialog = new ProductLookupView { DataContext = this };

            this.OnProductChosen += (chosenProduct) =>
            {
                // Prevent duplicate items in the SAME bill (optional check)
                if (PurchaseItems.Any(p => p.ProductId == chosenProduct.Id))
                {
                    MessageBox.Show("This item is already added to the bill. Just increase the quantity.");
                    dialog.Close();
                    return;
                }

                // Convert Product (Database Master) to PurchaseItem (Bill Row)
                var newItem = new PurchaseItem
                {
                    ProductId = chosenProduct.Id,
                    ItemName = chosenProduct.Name,
                    HSN = chosenProduct.HSN, // Assuming HSN is in Product Master
                    Qty = 1,
                    PricePerItem = chosenProduct.BasePrice,
                    TaxPercent = chosenProduct.GST // Default or fetched from Product Master
                };

                // Listen for price/qty changes for this specific row
                newItem.PropertyChanged += (s, e) => CalculateTotals();

                PurchaseItems.Add(newItem);
                dialog.Close();
            };

            dialog.ShowDialog();
        }

        private async Task ExecuteSave()
        {
            if (SelectedVendor == null || string.IsNullOrEmpty(InvoiceNo))
            {
                MessageBox.Show("Please select a vendor and enter an Invoice Number.");
                return;
            }

            var header = new PurchaseHeader
            {
                VendorId = SelectedVendor.Id,
                InvoiceNo = this.InvoiceNo,
                OriginalInvNo = this.OriginalInvNo,
                InvoiceDate = this.InvoiceDate,
                GrandTotal = this.GrandTotal,
                CGST = this.TotalCGST,
                SGST = this.TotalSGST,
                IGST = this.TotalIGST,
                TaxableAmount = this.TotalTaxable
            };

            try
            {
                bool success = await _repo.SaveFullPurchase(header, PurchaseItems.ToList());
                if (success) MessageBox.Show("Purchase Saved Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}

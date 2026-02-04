using BahiKitab.Models;
using BahiKitab.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for CreateOrderView.xaml
    /// </summary>
    public partial class CreateOrderView : UserControl
    {
        private ProductModel product = new ProductModel();

        private readonly ImagesDataService imagesDataService;
        private readonly LeadsDataService leadsDataService;

        public CreateOrderView()
        {
            InitializeComponent();
            imagesDataService = new ImagesDataService();
            leadsDataService = new LeadsDataService();
            this.Loaded += this.CreateOrderView_Loaded;
        }

        private async void CreateOrderView_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbCust.ItemsSource = await leadsDataService.GetAllLeadsAsync();
        }

        private async void btnUpload_Click(object sender, RoutedEventArgs e)
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
                    product.ImageIds.Add(await imagesDataService.CreateImageAsync(fileName, imageBytes));
                }

                this.tbImg.Text = product.ImageIds.Count + " images";
            }
        }

        public CostModel ProductCost
        {
            get { return (CostModel)GetValue(ProductCostProperty); }
            set { SetValue(ProductCostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Product.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductCostProperty =
            DependencyProperty.Register("ProductCost", typeof(CostModel), typeof(CreateOrderView), new PropertyMetadata(new CostModel()));

        public ObservableCollection<ProductModel> ProductCollection 
        {
            get { return (ObservableCollection<ProductModel>)GetValue(ProductCollectionProperty); }
            set { SetValue(ProductCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductCollectionProperty =
            DependencyProperty.Register("ProductCollection", typeof(ObservableCollection<ProductModel>), typeof(CreateOrderView), new PropertyMetadata(new ObservableCollection<ProductModel>()));

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbPname.Text) || string.IsNullOrWhiteSpace(this.tbPname.Text))
            {
                MessageBox.Show("Please mention product name", "Product Name", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            product.Name = this.tbPname.Text;
            product.ProductCost = this.ProductCost.Clone();
            product.CustomerId = ((Lead)this.cbCust.SelectedItem).Id;

            this.ProductCollection.Add(product.Clone());
            this.product = new ProductModel();
            this.ProductCost = new CostModel();
            this.tbImg.Text = "";
            this.tbPname.Text = "";
            this.cbCust.IsEnabled = false;
            this.btnAddCust.IsEnabled = false;
        }

        private void btnImg_Click(object sender, RoutedEventArgs e)
        {
            var window = new ImageViewer(product.ImageIds);
            window.Owner = Application.Current.MainWindow;
            window.Show();
        }

        private void btnOrderImg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

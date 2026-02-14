using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        private readonly OrderStagesDataService orderStagesDataService;

        private OrderViewModel viewModel;

        public OrderView()
        {
            InitializeComponent();
            orderStagesDataService = new OrderStagesDataService();

            this.Loaded += OrderView_Loaded;
        }

        private async void OrderView_Loaded(object sender, RoutedEventArgs e)
        {
            OrderStages = await orderStagesDataService.GetAllOrderStagesAsync();
            var dc = this.DataContext as OrderViewModel;
            if (dc != null) 
            {
                viewModel = dc;
            }
        }

        public ObservableCollection<OrderStageModel> OrderStages
        {
            get { return (ObservableCollection<OrderStageModel>)GetValue(OrderStagesProperty); }
            set { SetValue(OrderStagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderStages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderStagesProperty =
            DependencyProperty.Register("OrderStages", typeof(ObservableCollection<OrderStageModel>), typeof(OrderView), new PropertyMetadata(new ObservableCollection<OrderStageModel>()));
    }
}


using BahiKitab.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeadLeadsView.xaml
    /// </summary>
    public partial class DeadLeadsView : UserControl
    {
        private DeadLeadsViewModel viewModel;

        public DeadLeadsView()
        {
            InitializeComponent();
            this.Loaded += DeadLeadsView_Loaded;
            this.Unloaded += DeadLeadsView_Unloaded;
        }

        private void DeadLeadsView_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel?.SaveColumnSettings();
        }

        private void DeadLeadsView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as DeadLeadsViewModel;
            if (vm != null)
            {
                viewModel = vm;
            }
        }
    }
}

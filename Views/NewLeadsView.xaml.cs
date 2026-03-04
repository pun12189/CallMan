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
    public partial class NewLeadsView : UserControl
    {
        private NewLeadsViewModel viewModel;

        public NewLeadsView()
        {
            InitializeComponent();
            this.Loaded += NewLeadsView_Loaded;
            this.Unloaded += NewLeadsView_Unloaded;
        }

        private void NewLeadsView_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel?.SaveColumnSettings();
        }

        private void NewLeadsView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as NewLeadsViewModel;
            if (vm != null)
            {
                viewModel = vm;
            }
        }
    }
}

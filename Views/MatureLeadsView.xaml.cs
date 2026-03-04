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
    /// Interaction logic for LeadsView.xaml
    /// </summary>
    public partial class MatureLeadsView : UserControl
    {
        private MatureLeadsViewModel viewModel;

        public MatureLeadsView()
        {
            InitializeComponent();
            this.Loaded += MatureLeadsView_Loaded;
            this.Unloaded += MatureLeadsView_Unloaded;
        }

        private void MatureLeadsView_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel?.SaveColumnSettings();
        }

        private void MatureLeadsView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MatureLeadsViewModel;
            if (vm != null)
            {
                viewModel = vm;
            }
        }
    }
}

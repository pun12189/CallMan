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
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl
    {
        public TaskView()
        {
            InitializeComponent();
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid1.Items)
            {
                var row = this.dataGrid1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = true;
                }
            }
        }

        private void cbAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.dataGrid1.Items)
            {
                var row = this.dataGrid1.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = false;
                }
            }
        }
    }
}

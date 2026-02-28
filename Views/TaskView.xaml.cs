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
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl
    {
        private TaskViewModel dc;

        public TaskView()
        {
            InitializeComponent();
            this.Loaded += TaskView_Loaded;
        }

        private void TaskView_Loaded(object sender, RoutedEventArgs e)
        {
            var dc1 = this.DataContext as TaskViewModel;
            if (dc1 != null) 
            {
                dc = dc1;
            }
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

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dc.SelectedLead.Departments.Clear();
            dc.SelectedLead.Staff.Clear();
            foreach (var item in dc.AllDepartments)
            {
                if (item.IsSelected)
                {
                    dc.SelectedLead.Departments.Add(item);
                }
            }

            foreach (var item in dc.AllStaff)
            {
                if (item.IsSelected)
                {
                    dc.SelectedLead.Staff.Add(item);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class LeadsView : UserControl
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public LeadsView()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(System.AppDomain.CurrentDomain.BaseDirectory + @"TxtFile\Sample_Leads.csv", System.IO.Path.GetTempPath() + "\\Sample.csv", true);
            Process excel = new Process();
            excel.StartInfo.UseShellExecute = true;
            excel.StartInfo.FileName = System.IO.Path.GetTempPath() + "\\Sample.csv";
            excel.Start();

            // Need to wait for excel to start
            excel.WaitForInputIdle();

            IntPtr p = excel.MainWindowHandle;
            ShowWindow(p, 1);
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

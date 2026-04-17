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
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for SendBulkEmail.xaml
    /// </summary>
    public partial class SendBulkEmail : Window
    {
        public SendBulkEmail()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbSub.Text)) return;

            this.DialogResult = true;
        }
    }
}

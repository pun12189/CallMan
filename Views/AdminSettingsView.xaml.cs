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
    /// Interaction logic for AdminSettingsView.xaml
    /// </summary>
    public partial class AdminSettingsView : UserControl
    {
        public AdminSettingsView()
        {
            InitializeComponent();
        }

        public ICommand MainViewCommand
        {
            get { return (ICommand)GetValue(MainViewCommandProperty); }
            set { SetValue(MainViewCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainViewCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainViewCommandProperty =
            DependencyProperty.Register("MainViewCommand", typeof(ICommand), typeof(AdminSettingsView));
    }
}

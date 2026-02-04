using BahiKitab.Services;
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
    /// Interaction logic for AddStaffProfile.xaml
    /// </summary>
    public partial class AddStaffProfile : UserControl
    {
        private DepartmentsDataService departmentsData; 
        public AddStaffProfile()
        {
            InitializeComponent();
            this.Loaded += AddStaffProfile_Loaded;
            departmentsData = new DepartmentsDataService();
        }

        private async void AddStaffProfile_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is StaffViewModel)
            {
                pwdConfirm.Password = ((StaffViewModel)this.DataContext).CurrentLead.Password;
                pwdPassword.Password = ((StaffViewModel)this.DataContext).CurrentLead.Password;
            }

            this.cbDpt.ItemsSource = await departmentsData.GetAllDepartmentsAsync();
        }

        private void pwdPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pbox = sender as PasswordBox;
            if (pbox != null)
            {
                if (this.DataContext != null && this.DataContext is StaffViewModel)
                {
                    ((StaffViewModel)this.DataContext).CurrentLead.Password = pbox.Password;
                }
            }
        }
    }
}

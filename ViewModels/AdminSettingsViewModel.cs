using BahiKitab.Core;
using BahiKitab.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BahiKitab.ViewModels
{
    public class AdminSettingsViewModel : ViewModelBase
    {
        public ICommand NavigateCommand { get; }

        public AdminSettingsViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);            
        }

        private void Navigate(object parameter)
        {
            string viewName = parameter as string;

            switch (viewName)
            {
                case "Dept":
                    var window = new Window();
                    window.Content = new DepartmentView();
                    window.Show();
                    break;
                case "OStage":
                    var window1 = new Window();
                    window1.Content = new OrderStagesView();
                    window1.Show();
                    break;               
            }
        }
    }
}

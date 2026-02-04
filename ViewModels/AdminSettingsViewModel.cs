using BahiKitab.Core;
using BahiKitab.Helper;
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
        public ICommand NavigateCommand { get; set; }

        public CommonSettingsViewModel CommonSettingsViewModel { get; }

        public AdminSettingsViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);
            this.CommonSettingsViewModel = new CommonSettingsViewModel();
        }

        private void Navigate(object parameter)
        {
            var viewName = (ViewsEnum)parameter;
            if (viewName != null)
            {
                this.CommonSettingsViewModel.DynamicLoadViewCommand.Execute(viewName);
                this.CommonSettingsViewModel.DynamicViewCommand.Execute(viewName);
            }
        }
    }
}

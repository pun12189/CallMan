using BahiKitab.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace BahiKitab.ViewModels
{
    // A simple base class for all ViewModels, inheriting ObservableObject
    public class ViewModelBase : ObservableObject
    {
        
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => Set(ref _currentView, value, nameof(CurrentView));
        }

        public ICommand NavigateCommand { get; }

        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);

            // Set initial view
            CurrentView = new LeadsViewModel();
        }

        private void Navigate(object parameter)
        {
            string viewName = parameter as string;

            switch (viewName)
            {
                case "Leads":
                    CurrentView = new LeadsViewModel();
                    break;
                case "Tasks":
                    // CurrentView = new TasksViewModel(); // Placeholder for Task View
                    MessageBox.Show("Tasks View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Admin":
                    CurrentView = new AdminSettingsViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;               
            }
        }
    }
}

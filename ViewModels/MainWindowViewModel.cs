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
            CurrentView = new DashboardViewModel();
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
                case "Dead":
                    CurrentView = new DeadLeadsViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "New":
                    CurrentView = new NewLeadsViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Mature":
                    CurrentView = new MatureLeadsViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Dashboard":
                    CurrentView = new DashboardViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Orders":
                    CurrentView = new OrderViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
    }
}

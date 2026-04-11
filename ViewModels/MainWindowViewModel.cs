using BahiKitab.Core;
using BahiKitab.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public ICommand NavigateCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public MainWindowViewModel()
        {
            NavigateCommand = new RelayCommand(Navigate);
            LogoutCommand = new RelayCommand(LogoutCommandExecute);

            // Set initial view
            CurrentView = new DashboardViewModel();
        }

        private void LogoutCommandExecute(object obj)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout",
                                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // 1. Get a fresh LoginWindow from DI
                var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();

                Application.Current.MainWindow = loginWindow;

                // 2. Find the current MainWindow and trigger the "Safe" logout
                var currentWin = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (currentWin != null)
                {
                    currentWin.Logout(); // This sets the flag and closes the window
                }
            }
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
                    CurrentView = new TaskViewModel(); // Placeholder for Task View
                    //MessageBox.Show("Tasks View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Admin":
                    var adminSettingsViewModel = new AdminSettingsViewModel();
                    CurrentView = adminSettingsViewModel; // Placeholder for Admin Settings
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
                case "Staff":
                    CurrentView = new StaffViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Inventory":
                    CurrentView = new InventoryViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Category":
                    CurrentView = new CategoryViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Profile":
                    CurrentView = new ProfileViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Purchase":
                    CurrentView = new PurchaseViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Smtp":
                    CurrentView = new EmailSettingsViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case "Workflow":
                    CurrentView = new WorkflowViewModel(); // Placeholder for Admin Settings
                    //MessageBox.Show("Admin View not fully implemented yet.", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }
    }
}

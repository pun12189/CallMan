using BahiKitab.Core;
using BahiKitab.Services.Interface;
using BahiKitab.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BahiKitab.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private string _username;
        private string _errorMessage;
        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value, nameof(IsBusy));
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value, nameof(Username));
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value, nameof(ErrorMessage));
        }

        public ICommand LoginCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand ExitCommand { get; }

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService;

            LoginCommand = new RelayCommand(ExecuteLogin);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgot);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        private async void ExecuteLogin(object parameter)
        {
            if (IsBusy) return; // Prevent double-clicking

            IsBusy = true; // Show Spinner, Disable Button

            try
            {
                string? password = (parameter as IHavePassword)?.Password;

                // This runs on a background thread; UI stays alive!
                bool isAuthenticated = await _authService.AuthenticateAsync(Username, password);

                if (isAuthenticated)
                {
                    var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();

                    // 2. Set the new window as the actual MainWindow of the app
                    Application.Current.MainWindow = mainWindow;

                    // 3. Now it is safe to close the login window
                    CloseCurrentWindow();

                    // 4. (Optional) Set mode back to default if you want app to close when MainWindow closes
                    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                }
                else
                {
                    ErrorMessage = "Invalid credentials.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Connection failed: " + ex.Message;
            }
            finally
            {
                IsBusy = false; // Hide Spinner, Enable Button
            }           
        }

        private void ExecuteForgot(object parameter)
        {
            _authService.RequestPasswordReset(Username);
            MessageBox.Show("Password reset instructions sent.");
        }

        private void CloseCurrentWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            window?.Close();
        }
    }
}

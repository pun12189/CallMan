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

namespace BahiKitab.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authService;
        private string _username;
        private string _errorMessage;

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

        private void ExecuteLogin(object parameter)
        {
            var passwordContainer = parameter as IHavePassword;
            string password = passwordContainer?.Password;

            if (_authService.Authenticate(Username, password))
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
                ErrorMessage = "Invalid username or password.";
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

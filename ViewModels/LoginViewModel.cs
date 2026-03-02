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
        private string _resetEmail;
        private bool isLoginVisible = true;
        private bool isForgotVisible = false;

        public bool IsLoginVisible { get => isLoginVisible; set => Set(ref isLoginVisible, value, nameof(IsLoginVisible)); }

        public bool IsForgotVisible { get => isForgotVisible; set => Set(ref isForgotVisible, value, nameof(IsForgotVisible)); }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value, nameof(IsBusy));
        }

        public string ResetEmail
        {
            get => _resetEmail;
            set => Set(ref _resetEmail, value, nameof(ResetEmail));
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
        public ICommand ExitCommand { get; }
        public ICommand SwitchViewCommand { get; }
        public ICommand SendResetCommand { get; }

        public LoginViewModel(IAuthenticationService authService)
        {
            _authService = authService;

            LoginCommand = new RelayCommand(ExecuteLogin);
            SwitchViewCommand = new RelayCommand(SwitchViewCommandExecute);
            SendResetCommand = new RelayCommand(SendResetCommandExecute);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }

        private async void SendResetCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(ResetEmail)) return;

            IsBusy = true; // Show your professional spinner!
            var success = await _authService.ResetPasswordAsync(ResetEmail);
            IsBusy = false;

            if (success)
            {
                if (MessageBox.Show("A temporary password has been sent to your email.") == MessageBoxResult.OK)
                {
                    IsLoginVisible = !IsLoginVisible;
                    IsForgotVisible = !IsForgotVisible;
                    ErrorMessage = ""; // Clear any old errors when switching
                }                
            }
            else
            {
                ErrorMessage = "Email not found in our records.";
            }
        }

        private void SwitchViewCommandExecute(object obj)
        {
            IsLoginVisible = !IsLoginVisible;
            IsForgotVisible = !IsForgotVisible;
            ErrorMessage = ""; // Clear any old errors when switching
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

        private void CloseCurrentWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
            window?.Close();
        }
    }
}

using BahiKitab.Core;
using BahiKitab.Services;
using BahiKitab.Services.Interface;
using BahiKitab.ViewModels;
using BahiKitab.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BahiKitab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<IPasswordService, BCryptPasswordService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainWindowViewModel>();

            // Windows
            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Show the Splash Screen immediately
            var splash = new SplashWindow();
            splash.Show();

            // 2. Simulate or perform heavy initialization (e.g., Database migrations, DI building)
            // We use Task.Run to keep the UI responsive
            await Task.Run(() =>
            {
                // This is where you'd put heavy service init logic if needed
                System.Threading.Thread.Sleep(4000); // Artificial delay to show the splash
            });

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            // 3. Resolve the Login Window from our built ServiceProvider
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();

            // 4. Show Login and Close Splash
            loginWindow.Show();
            splash.Close();
        }
    }

}

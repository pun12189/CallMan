using BahiKitab.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isLoggingOut = false;

        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        // Call this from ViewModel before calling .Close()
        public void Logout()
        {
            _isLoggingOut = true;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // If we aren't logging out, the user clicked 'X' or Alt+F4
            // Since we are in ExplicitShutdown mode, we MUST call Shutdown()
            if (!_isLoggingOut)
            {
                if (Application.Current.Windows.Count == 0)
                {
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
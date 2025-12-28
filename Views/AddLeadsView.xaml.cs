using System.Windows;
using System.Windows.Controls;
using BahiKitab.Models;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for AddLeadsView.xaml
    /// </summary>
    public partial class AddLeadsView : UserControl
    {
        public AddLeadsView()
        {
            InitializeComponent();
            List<string> MyStringList = new List<string> { "New", "Contacted", "Proposal", "Matured", "Qualified" };
            this.cbStatus.ItemsSource = MyStringList;
        }
    }
}

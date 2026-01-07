using BahiKitab.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitab.Views
{
    /// <summary>
    /// Interaction logic for LeadStatusView.xaml
    /// </summary>
    public partial class LeadStatusView : UserControl
    {
        private readonly FollowUpStagesDataService followUpStagesDataService;
        private readonly DeadReasonsDataService deadReasonsDataService;

        public LeadStatusView()
        {
            InitializeComponent();
            this.followUpStagesDataService = new FollowUpStagesDataService();
            this.deadReasonsDataService = new DeadReasonsDataService();

            this.Loaded += LeadStatusView_Loaded;
        }

        private async void LeadStatusView_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbReason.ItemsSource = await this.deadReasonsDataService.GetAllDeadReasonsAsync();
            this.cbFollowup.ItemsSource = await this.followUpStagesDataService.GetAllFollowUpStagessAsync();
        }
    }
}

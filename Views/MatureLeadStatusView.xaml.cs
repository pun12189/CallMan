using BahiKitab.Models;
using BahiKitab.Services;
using BahiKitab.ViewModels;
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
    public partial class MatureLeadStatusView : UserControl
    {
        private readonly MatureStagesDataService followUpStagesDataService;
        private readonly DeadReasonsDataService deadReasonsDataService;
        private readonly LeadsOrderDataService leadsOrderDataService;
        private readonly LeadHistoryDataService leadHistoryDataService;

        private MatureLeadsViewModel? leadsViewModel;

        private readonly LeadHistoryModel leadHistoryModel;

        public MatureLeadStatusView()
        {
            InitializeComponent();
            this.followUpStagesDataService = new MatureStagesDataService();
            this.deadReasonsDataService = new DeadReasonsDataService();
            this.leadHistoryDataService = new LeadHistoryDataService();
            this.leadsOrderDataService = new LeadsOrderDataService();
            this.leadHistoryModel = new LeadHistoryModel();
            this.Loaded += LeadStatusView_Loaded;
        }

        private async void LeadStatusView_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbReason.ItemsSource = await this.deadReasonsDataService.GetAllDeadReasonsAsync();
            this.cbFollowup.ItemsSource = await this.followUpStagesDataService.GetAllMatureStagessAsync();
            this.leadsViewModel = this.DataContext as MatureLeadsViewModel;            
        }

        private async void UpdateLeadStatus(object sender, RoutedEventArgs e)
        {
            if (this.rdDead.IsChecked != null && this.rdDead.IsChecked == true)
            {
                if (this.leadsViewModel != null && this.leadsViewModel.SelectedLead != null)
                {
                    this.leadsViewModel.SelectedLead.LeadType = Helper.LeadType.Dead;
                    var deadModel = new LeadDeadModel();
                    deadModel.CreateDead = DateTime.Now;
                    var dStage = this.cbReason.SelectedItem as DeadReasonModel;
                    if (dStage != null)
                    {
                        deadModel.Name = dStage.Name;
                    }                    

                    if (!string.IsNullOrEmpty(this.tbDMsg.Text) && !string.IsNullOrWhiteSpace(this.tbDMsg.Text))
                    {
                        deadModel.LastMsg = this.tbDMsg.Text;
                    }

                    deadModel.LeadId = this.leadsViewModel.SelectedLead.Id;

                    this.leadsViewModel.SelectedLead.LeadDeadModel = deadModel;

                    this.leadHistoryModel.LeadId = deadModel.LeadId;
                    this.leadHistoryModel.Name = deadModel.Name;
                    this.leadHistoryModel.LeadType = Helper.LeadType.Dead;
                    this.leadHistoryModel.LastMsg = deadModel.LastMsg;

                    await leadHistoryDataService.CreateLeadHistoryAsync(this.leadHistoryModel);
                }

                this.CloseWindow();
            }
            else if (this.rdFollow.IsChecked != null && this.rdFollow.IsChecked == true)
            {
                if (this.leadsViewModel != null && this.leadsViewModel.SelectedLead != null)
                {
                    // this.leadsViewModel.SelectedLead.LeadType = Helper.LeadType.Matured;
                    var orderModel = new LeadOrderModel();
                    if (!string.IsNullOrEmpty(this.tbOVal.Text) && !string.IsNullOrWhiteSpace(this.tbOVal.Text))
                    {
                        orderModel.OrderAmount = Convert.ToDouble(this.tbOVal.Text);
                    }

                    if (!string.IsNullOrEmpty(this.tbRVal.Text) && !string.IsNullOrWhiteSpace(this.tbRVal.Text))
                    {
                        orderModel.ReceivedAmount = Convert.ToDouble(this.tbRVal.Text);
                    }

                    orderModel.Balance = orderModel.OrderAmount - orderModel.ReceivedAmount;

                    if (orderModel.Balance <= 0.0)
                    {
                        orderModel.PaymentStatus = Helper.PaymentStatus.Paid;
                        orderModel.PaymentType = Helper.PaymentType.Cash;
                    }
                    else if (orderModel.Balance == orderModel.OrderAmount)
                    {
                        orderModel.PaymentStatus = Helper.PaymentStatus.Unpaid;
                        orderModel.PaymentType = Helper.PaymentType.Credit;
                    }
                    else
                    {
                        orderModel.PaymentStatus = Helper.PaymentStatus.PartialPaid;
                        orderModel.PaymentType= Helper.PaymentType.Credit;
                    }

                    var nextFDate = this.dtNFollow2.SelectedDateTime;
                    if (nextFDate != null)
                    {
                        orderModel.NextFollowup = (DateTime)nextFDate;
                    }

                    if (!string.IsNullOrEmpty(this.tbLMsg.Text) && !string.IsNullOrWhiteSpace(this.tbLMsg.Text))
                    {
                        orderModel.LastMsg = this.tbLMsg.Text;
                    }

                    orderModel.Customer = this.leadsViewModel.SelectedLead;

                    orderModel.OrderId = "LKY" + DateTime.Now.GetHashCode();
                    orderModel.IsAccepted = false;

                    this.leadHistoryModel.LeadId = orderModel.Customer.Id;
                    this.leadHistoryModel.NextFollowUp = orderModel.NextFollowup;
                    this.leadHistoryModel.Name = "Received Order of Amount: " + orderModel.OrderAmount + Environment.NewLine + "Received Amount: " + orderModel.ReceivedAmount +  Environment.NewLine + "Order ID: " + orderModel.OrderId;
                    this.leadHistoryModel.LeadType = Helper.LeadType.Matured;
                    this.leadHistoryModel.LastMsg = orderModel.LastMsg;
                    
                    await leadsOrderDataService.CreateLeadOrderAsync(orderModel);
                    await leadHistoryDataService.CreateLeadHistoryAsync(this.leadHistoryModel);
                }

                this.CloseWindow();
            }
            else
            {
                MessageBox.Show("Please select one option to update the lead", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CloseWindow()
        {
            var window = this.Parent as Window;
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
    }
}

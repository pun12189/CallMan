using BahiKitab.Models;
using BahiKitab.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        // Services
        private readonly LeadsDataService _dataService;

        // Data Properties
        private ObservableCollection<Lead> _leads;
        private string totalLeads;
        private string newLeads;
        private string deadLeads;
        private string matureLeads;
        private string totalOrders;
        private string paymentReceived;
        private string followUpLeads;
        private string noUpdation;

        public ObservableCollection<Lead> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        public string TotalLeads { get => totalLeads; set => Set(ref totalLeads, value, nameof(TotalLeads)); }

        public string NewLeads { get => newLeads; set => Set(ref newLeads, value, nameof(NewLeads)); }

        public string DeadLeads { get => deadLeads; set => Set(ref deadLeads, value, nameof(DeadLeads)); }
        public string MatureLeads { get => matureLeads; set => Set(ref matureLeads, value, nameof(MatureLeads)); }
        public string TotalOrders { get => totalOrders; set => Set(ref totalOrders, value, nameof(TotalOrders)); }
        public string PaymentReceived { get => paymentReceived; set => Set(ref paymentReceived, value, nameof(PaymentReceived)); }
        public string FollowUpLeads { get => followUpLeads; set => Set(ref followUpLeads, value, nameof(FollowUpLeads)); }
        public string NoUpdation { get => noUpdation; set => Set(ref noUpdation, value, nameof(NoUpdation)); }

        public DashboardViewModel() 
        {
            _dataService = new LeadsDataService();
            Leads = new ObservableCollection<Lead>();

            GetAllLeads();
            
        }

        private async Task GetAllLeads()
        {
            Leads = await _dataService.GetAllLeadsAsync();

            this.TotalLeads = Leads.Count + Environment.NewLine + "Total Leads";

            this.NewLeads = Leads.Count(l => l.LeadType == Helper.LeadType.New) + Environment.NewLine + "New Leads";

            this.DeadLeads = Leads.Count(l => l.LeadType == Helper.LeadType.Dead) + Environment.NewLine + "Dead Leads";

            this.FollowUpLeads = Leads.Count(l => l.LeadType == Helper.LeadType.FollowUp) + Environment.NewLine + "Follow Up";

            this.MatureLeads = Leads.Count(l => l.LeadType == Helper.LeadType.Matured) + Environment.NewLine + "Customers";
        }

    }
}

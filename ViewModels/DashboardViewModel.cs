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
        private readonly LeadsOrderDataService leadsOrderDataService;

        // Data Properties
        private ObservableCollection<Lead> _leads;
        private double totalLeads;
        private double newLeads;
        private double deadLeads;
        private double matureLeads;
        private double totalOrders;
        private double paymentReceived;
        private double followUpLeads;
        private double noUpdation;
        private ObservableCollection<LeadOrderModel> _leadOrders;

        public ObservableCollection<Lead> Leads
        {
            get => _leads;
            set => Set(ref _leads, value, nameof(Leads));
        }

        public ObservableCollection<LeadOrderModel> LeadOrders
        {
            get => _leadOrders;
            set => Set(ref _leadOrders, value, nameof(LeadOrders));
        }

        public double TotalLeads { get => totalLeads; set => Set(ref totalLeads, value, nameof(TotalLeads)); }

        public double NewLeads { get => newLeads; set => Set(ref newLeads, value, nameof(NewLeads)); }

        public double DeadLeads { get => deadLeads; set => Set(ref deadLeads, value, nameof(DeadLeads)); }
        public double MatureLeads { get => matureLeads; set => Set(ref matureLeads, value, nameof(MatureLeads)); }
        public double TotalOrders { get => totalOrders; set => Set(ref totalOrders, value, nameof(TotalOrders)); }
        public double PaymentReceived { get => paymentReceived; set => Set(ref paymentReceived, value, nameof(PaymentReceived)); }
        public double FollowUpLeads { get => followUpLeads; set => Set(ref followUpLeads, value, nameof(FollowUpLeads)); }
        public double NoUpdation { get => noUpdation; set => Set(ref noUpdation, value, nameof(NoUpdation)); }

        public DashboardViewModel() 
        {
            _dataService = new LeadsDataService();
            leadsOrderDataService = new LeadsOrderDataService();
            Leads = new ObservableCollection<Lead>();
            LeadOrders = new ObservableCollection<LeadOrderModel>();

            GetAllLeads();
            
        }

        private async Task GetAllLeads()
        {
            Leads = await _dataService.GetAllLeadsAsync();
            LeadOrders = await leadsOrderDataService.GetAllOrdersAsync();

            this.TotalLeads = Leads.Count;

            this.NewLeads = Leads.Count(l => l.LeadType == Helper.LeadType.New);

            this.DeadLeads = Leads.Count(l => l.LeadType == Helper.LeadType.Dead);

            this.FollowUpLeads = Leads.Count(l => l.LeadType == Helper.LeadType.FollowUp);

            this.MatureLeads = Leads.Count(l => l.LeadType == Helper.LeadType.Matured);

            this.TotalOrders = LeadOrders.Count();
        }

    }
}

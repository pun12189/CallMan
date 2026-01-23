using BahiKitab.Core;
using BahiKitab.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitab.Models
{
    public class LeadOrderModel : ObservableObject, ICloneable
    {
        private int id;
        private string orderId;
        private PaymentType paymentType;
        private double receivedAmount;
        private double balance;
        private Lead customer;
        private PaymentStatus paymentStatus;
        private OrderStageModel orderStatus;
        private string takenBy;
        private DateTime created;
        private DateTime updated;
        private ObservableCollection<InventoryModel> orderedProducts;
        private DateTime nextFollowup;
        private double orderAmount;
        private DateTime acceptedDate;
        private bool priority;
        private bool isAccepted;
        private double discount;
        private string lastMsg;

        public int Id { get => id; set => Set(ref id, value, nameof(Id)); }

        public string OrderId { get => orderId; set => Set(ref orderId, value, nameof(OrderId)); }

        public PaymentType PaymentType { get => paymentType; set => Set(ref paymentType, value, nameof(PaymentType)); }

        public double ReceivedAmount { get => receivedAmount; set => Set(ref receivedAmount, value, nameof(ReceivedAmount)); }

        public double Balance { get => balance; set => Set(ref balance, value, nameof(Balance)); }

        public Lead Customer { get => customer; set => Set(ref customer, value, nameof(Customer)); }

        public PaymentStatus PaymentStatus { get => paymentStatus; set => Set(ref paymentStatus, value, nameof(PaymentStatus)); }

        public OrderStageModel OrderStatus { get => orderStatus; set => Set(ref orderStatus, value, nameof(OrderStatus)); }

        public string TakenBy { get => takenBy; set => Set(ref takenBy, value, nameof(TakenBy)); }

        public DateTime Created { get => created; set => Set(ref created, value, nameof(Created)); }

        public DateTime Updated { get => updated; set => Set(ref updated, value, nameof(Updated)); }

        public ObservableCollection<InventoryModel> OrderedProducts { get => orderedProducts; set => Set(ref orderedProducts, value, nameof(OrderedProducts)); }

        public DateTime NextFollowup { get => nextFollowup; set => Set(ref nextFollowup, value, nameof(NextFollowup)); }

        public double OrderAmount { get => orderAmount; set => Set(ref orderAmount, value, nameof(OrderAmount)); }

        public DateTime AcceptedDate { get => acceptedDate; set => Set(ref acceptedDate, value, nameof(AcceptedDate)); }

        public bool Priority { get => priority; set => Set(ref priority, value, nameof(Priority)); }

        public bool IsAccepted { get => isAccepted; set => Set(ref isAccepted, value, nameof(IsAccepted)); }

        public double Discount { get => discount; set => Set(ref discount, value, nameof(Discount)); }

        public string LastMsg { get => lastMsg; set => Set(ref lastMsg, value, nameof(LastMsg)); }

        public LeadOrderModel Clone() { return (LeadOrderModel)this.MemberwiseClone(); }

        object ICloneable.Clone() { return Clone(); }
    }
}

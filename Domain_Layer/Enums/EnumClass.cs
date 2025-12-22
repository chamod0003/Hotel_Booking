using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Enums
{
    public static class BookingStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Cancelled = "Cancelled";
        public const string Completed = "Completed";
        public const string NoShow = "NoShow";
    }

    public static class PaymentStatus
    {
        public const string Pending = "Pending";
        public const string Paid = "Paid";
        public const string Failed = "Failed";
        public const string Refunded = "Refunded";
        public const string PartiallyRefunded = "PartiallyRefunded";
    }

    public static class PaymentMethod
    {
        public const string CreditCard = "CreditCard";
        public const string DebitCard = "DebitCard";
        public const string Cash = "Cash";
        public const string BankTransfer = "BankTransfer";
        public const string OnlinePayment = "OnlinePayment";
    }
}

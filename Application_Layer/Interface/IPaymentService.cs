using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Interface
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessBookingPaymentAsync(int bookingId, PaymentRequest request);
        Task<PaymentResult> RefundBookingPaymentAsync(int bookingId, string reason);
        Task<bool> ValidatePaymentAsync(string transactionId);
        IEnumerable<string> GetAvailablePaymentMethods();
    }
}

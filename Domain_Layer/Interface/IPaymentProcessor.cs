using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IPaymentProcessor
    {

        string PaymentMethodName { get; }

        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);

        Task<PaymentResult> RefundPaymentAsync(string transactionId, decimal amount);

        Task<bool> ValidatePaymentAsync(string transactionId);
    }
}

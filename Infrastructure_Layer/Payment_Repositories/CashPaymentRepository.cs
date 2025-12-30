using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Payment_Repositories
{
    public class CashPaymentRepository : IPaymentProcessor
    {
        public string PaymentMethodName => "Cash";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            Console.WriteLine($" Processing Cash Payment: LKR {request.Amount}");
            await Task.Delay(500);

            var transactionId = $"CASH-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = transactionId,
                Message = "Cash payment - To be collected at hotel",
                ProcessedAt = DateTime.UtcNow,
                AdditionalData = new Dictionary<string, string>
                {
                    { "PaymentType", "Cash on Arrival" },
                    { "Note", "Payment will be collected at hotel reception" }
                }
            };
        }

        public async Task<PaymentResult> RefundPaymentAsync(string transactionId, decimal amount)
        {
            await Task.Delay(500);
            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = $"REFUND-{transactionId}",
                Message = "Refund processed",
                ProcessedAt = DateTime.UtcNow
            };
        }

        public async Task<bool> ValidatePaymentAsync(string transactionId)
        {
            await Task.Delay(100);
            return transactionId.StartsWith("CASH-");


        }
    }
}

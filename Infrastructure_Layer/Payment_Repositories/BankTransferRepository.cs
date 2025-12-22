using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Payment_Repositories
{
    public class BankTransferRepository : IPaymentProcessor
    {
        public string PaymentMethodName => "BankTransfer";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            Console.WriteLine($"🏦 Processing Bank Transfer: LKR {request.Amount}");
            await Task.Delay(1000);

            var transactionId = $"BT-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = transactionId,
                Message = "Bank transfer initiated - Pending verification",
                ProcessedAt = DateTime.UtcNow,
                AdditionalData = new Dictionary<string, string>
                {
                    { "Status", "Pending" },
                    { "BankName", request.BankName ?? "Not specified" }
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
                Message = "Bank transfer refund initiated",
                ProcessedAt = DateTime.UtcNow
            };
        }

        public async Task<bool> ValidatePaymentAsync(string transactionId)
        {
            await Task.Delay(100);
            return transactionId.StartsWith("BT-");
        }
    }
}

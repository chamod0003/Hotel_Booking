using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;

namespace Infrastructure_Layer.Payment_Repositories
{
    public class DebitCardPaymentRepository:IPaymentProcessor
    {
        public string PaymentMethodName => "DebitCard";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            Console.WriteLine($" Processing Debit Card Payment: LKR {request.Amount}");


            var transactionId = $"DC-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = transactionId,
                Message = "Debit card payment successful",
                ProcessedAt = DateTime.UtcNow
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
            return transactionId.StartsWith("DC-");
        }
    }
}

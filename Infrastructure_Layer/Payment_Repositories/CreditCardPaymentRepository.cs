using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using System.Runtime.CompilerServices;

namespace Infrastructure_Layer.Payment_Repositories
{
    public class CreditCardPaymentRepository: IPaymentProcessor
    {
    
        string IPaymentProcessor.PaymentMethodName => "CreditCard";

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                Console.WriteLine($"Processing Credit Card Payment: LKR {request.Amount}");

                await Task.Delay(1000);

                if (string.IsNullOrEmpty(request.CardNumber) || (request.CardNumber.Length < 16))
                {
                    return new PaymentResult
                    {
                        IsSuccess = false,
                        Message = "Invalid Number",
                        ErrorCode = "Invalid Card",
                        ProcessedAt = DateTime.Now,
                    };
                }

                var transactionId = $"CC-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";

                return new PaymentResult
                {
                    IsSuccess = true,
                    Message = "Payment Processed Successfully",
                    ErrorCode = null,
                    ProcessedAt = DateTime.Now,
                    TransactionId = transactionId,
                    AdditionalData = new Dictionary<string, string>
                    {
                        { "CardLastFour", request.CardNumber.Substring(request.CardNumber.Length - 4) },
                        { "PaymentMethod", "Credit Card" }

                    }

                };


            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = $"Payment failed: {ex.Message}",
                    ErrorCode = "PAYMENT_ERROR",
                    ProcessedAt = DateTime.UtcNow
                };
            }
        }

        public async Task<PaymentResult> RefundPaymentAsync(string transactionId, decimal amount)
        {
            Console.WriteLine($" Refunding Credit Card: {transactionId} - LKR {amount}");

            await Task.Delay(500);

            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = $"REFUND-{transactionId}",
                Message = "Refund processed successfully",
                ProcessedAt = DateTime.UtcNow
            };
                

        }

        public async Task<bool> ValidatePaymentAsync(string transactionId)
        {
            await Task.Delay(500);
            return transactionId.StartsWith("CC-");
        }
    }
}

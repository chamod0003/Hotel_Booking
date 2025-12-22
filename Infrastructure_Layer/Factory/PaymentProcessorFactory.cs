using Domain_Layer.Interface;
using Infrastructure_Layer.Payment_Repositories;

namespace Infrastructure_Layer.Factory
{
    public class PaymentProcessorFactory
    {
        private readonly Dictionary<string, IPaymentProcessor> repository;

        public PaymentProcessorFactory()
        {
            repository = new Dictionary<string, IPaymentProcessor>(StringComparer.OrdinalIgnoreCase)
            {
                { "CreditCard", new CreditCardPaymentRepository() },
                { "DebitCard", new DebitCardPaymentRepository() },
                { "Cash", new CashPaymentRepository() },
                { "BankTransfer", new BankTransferRepository() }
            };
        }

        public IPaymentProcessor? GetPaymentProcessor(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method cannot be empty");
            }

            if (repository.TryGetValue(paymentMethod, out var processor))
            {
                return processor;
            }
            return null;
        }

        public IEnumerable<string> GetSupportedPaymentMethods()
        {
            return repository.Keys;
        }
    }
}

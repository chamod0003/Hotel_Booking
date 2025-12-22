using Application_Layer.Interface;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;

        private readonly IBookingRepository bookingRepository;

        private readonly PaymentProcessorFactory  paymentProcessorFactory;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository, PaymentProcessorFactory paymentProcessorFactory)
        {
            this.paymentRepository = paymentRepository;
            this.bookingRepository = bookingRepository;
            this.paymentProcessorFactory = paymentProcessorFactory;
        }


        public async Task<PaymentResult> ProcessBookingPaymentAsync(int bookingId, PaymentRequest request)
        {
            var booking = await bookingRepository.GetByIdBookingAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            var processor = paymentProcessorFactory.GetPaymentProcessor(booking.PaymentMethod);

            request.Amount = booking.TotalAmount;
            request.BookingReference = booking.BookingReference;
            request.CustomerName = booking.User.FullName;
            request.CustomerEmail = booking.User.Email;

            var result = await processor.ProcessPaymentAsync(request);

            if (result.IsSuccess)
            {
                var payment = new Payment
                {
                    BookingId = bookingId,
                    Amount = booking.TotalAmount,
                    PaymentMethod = booking.PaymentMethod,
                    PaymentStatus = "Paid",
                    TransactionId = result.TransactionId,
                    PaymentDate = DateTime.UtcNow
                };

                await paymentRepository.CreatePaymentAsync(payment);

                await bookingRepository.UpdatePaymentStatusAsync(
                    bookingId,
                    "Paid",
                    result.TransactionId);
            }

            return result;
        }

        public async Task<PaymentResult> RefundBookingPaymentAsync(int bookingId, string reason)
        {
            var booking = await bookingRepository.GetByIdBookingAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            if (string.IsNullOrEmpty(booking.PaymentTransactionId))
                throw new Exception("No payment transaction found");

            var processor = paymentProcessorFactory.GetPaymentProcessor(booking.PaymentMethod);

            var result = await processor.RefundPaymentAsync(
                booking.PaymentTransactionId,
                booking.TotalAmount);

            if (result.IsSuccess)
            {
                booking.PaymentStatus = "Refunded";
                booking.RefundAmount = booking.TotalAmount;
                await bookingRepository.UpdateBookingAsync(booking);

                // Save refund payment record
                var refundPayment = new Payment
                {
                    BookingId = bookingId,
                    Amount = -booking.TotalAmount,
                    PaymentMethod = booking.PaymentMethod,
                    PaymentStatus = "Refunded",
                    TransactionId = result.TransactionId,
                    PaymentDate = DateTime.UtcNow,
                    Notes = reason
                };

                await paymentRepository.CreatePaymentAsync(refundPayment);
            }

            return result;
        }

        public async Task<bool> ValidatePaymentAsync(string transactionId)
        {
            var paymentMethod = transactionId.Split('-')[0] switch
            {
                "CC" => "CreditCard",
                "DC" => "DebitCard",
                "CASH" => "Cash",
                "BT" => "BankTransfer",
                _ => throw new Exception("Unknown transaction ID format")
            };

            var processor = paymentProcessorFactory.GetPaymentProcessor(paymentMethod);
            return await processor.ValidatePaymentAsync(transactionId);
        }

        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            return paymentProcessorFactory.GetSupportedPaymentMethods();
        }
    }
}

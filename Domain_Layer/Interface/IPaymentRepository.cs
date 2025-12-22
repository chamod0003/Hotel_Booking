using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);

        Task<Payment> GetPaymentByIdAsync(int paymentId);

        Task<IEnumerable<Payment>> GetAllPaymentsByBookingIdAsync(int BookingId);

        Task<Payment> UpdatePaymentAsync(Payment payment);
    }
}

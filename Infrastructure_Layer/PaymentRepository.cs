using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer
{
    public class PaymentRepository :IPaymentRepository
    {
        private readonly AppDbContext appDbContext;

        public PaymentRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            var result = await appDbContext.payments.AddAsync(payment);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsByBookingIdAsync(int BookingId)
        {
                 var result = await appDbContext.payments
                .Where(p => p.BookingId == BookingId)
                .ToListAsync();
            return result;

        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            var result = await appDbContext.payments
                            .Include(p => p.BookingId)
                            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            return result;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            appDbContext.payments.Update(payment);
            await appDbContext.SaveChangesAsync();
            return payment;
        }
    }
}

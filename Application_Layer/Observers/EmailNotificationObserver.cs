using Application_Layer.Interface;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Observers
{
    public class EmailNotificationObserver:IBookingObserver
    {
        private readonly IEmailService emailService;

        public EmailNotificationObserver(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public async Task OnBookingCancelledAsync(Booking booking)
        {
            await emailService.SendBookingCancellationEmailAsync(booking);
        }

        public async Task OnBookingConfirmedAsync(Booking booking)
        {
            await emailService.SendBookingConfirmationEmailAsync(booking);
        }

        public async Task OnBookingCreatedAsync(Booking booking)
        {
            await emailService.SendBookingConfirmationEmailAsync(booking);
        }

        public async Task OnPaymentCompletedAsync(Booking booking, Payment payment)
        {
            await emailService.SendPaymentConfirmationEmailAsync(payment, booking);
        }
    }
}

using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IBookingObserver
    {
        Task OnBookingCreatedAsync (Booking booking);

        Task OnBookingCancelledAsync (Booking booking);

        Task OnBookingConfirmedAsync (Booking booking);

        Task OnPaymentCompletedAsync (Booking booking, Payment payment);
    }
}

using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IBookingSubject
    {
        void Attach(IBookingObserver observer);

        void Detach(IBookingObserver observer);

        Task NotifyBookingCreatedAsync (Booking booking);

        Task NotifyBookingConfirmedAsync (Booking booking);

        Task NotifyBookingCancelledAsync (Booking booking);

        Task NotifyPaymentCompletedAsync (Booking booking, Payment payment);
    }
}

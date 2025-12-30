using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class BookingSubject : IBookingSubject
    {
        private readonly List<IBookingObserver> _observers = new();
        private readonly ILogger<BookingSubject> _logger;

        public BookingSubject(ILogger<BookingSubject> logger)
        {
            _logger = logger;
        }

        public void Attach(IBookingObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                _logger.LogInformation($"✅ Observer attached: {observer.GetType().Name}");
            }
        }

        public void Detach(IBookingObserver observer)
        {
            _observers.Remove(observer);
        }

        public async Task NotifyBookingCreatedAsync(Booking booking)
        {
            _logger.LogInformation($"📢 Notifying {_observers.Count} observer(s): Booking created - {booking.BookingReference}");

            foreach (var observer in _observers)
            {
                await observer.OnBookingCreatedAsync(booking);
            }
        }

        public async Task NotifyBookingConfirmedAsync(Booking booking)
        {
            foreach (var observer in _observers)
            {
                await observer.OnBookingConfirmedAsync(booking);
            }
        }

        public async Task NotifyBookingCancelledAsync(Booking booking)
        {
            foreach (var observer in _observers)
            {
                await observer.OnBookingCancelledAsync(booking);
            }
        }

        public async Task NotifyPaymentCompletedAsync(Booking booking, Payment payment)
        {
            _logger.LogInformation($"📢 Notifying observers: Payment completed - {payment.TransactionId}");

            foreach (var observer in _observers)
            {
                await observer.OnPaymentCompletedAsync(booking, payment);
            }
        }
    }
}

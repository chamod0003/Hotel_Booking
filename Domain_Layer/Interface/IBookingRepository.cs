using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interface
{
    public interface IBookingRepository
    {
        //create karana eka
        Task<Booking> CreateBookingAsync(Booking booking);

        //get karana eka
        Task<Booking> GetByIdBookingAsync(int BookingId);
        Task<Booking> GetByReferncesAsync(string BookingRefernces);
        Task<IEnumerable<Booking>> GetAllBookingAsync();
        Task<IEnumerable<Booking>> GetBookingByUserIdAsync(Guid UserId);
        Task<IEnumerable<Booking>> GetBookingsByHotelIdAsync(int HotelId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string Status);

        Task<IEnumerable<Booking>> GetUpcomingBookingsAsync();



        //update karana eka

        Task<Booking> UpdateBookingAsync(Booking booking);
        Task<bool> UpdateBookingStatusAsync(int BookingId, string Status);
        Task<bool> UpdatePaymentStatusAsync(int BookingId, string paymentStatus, string transactionId);


        //delete karana eka

        Task<bool> DeleteBookingAsync(int BookingId);
        Task<bool> CancelBookingAsync(int BookingId, string reason);

        //check availability

        Task<bool> IsRoomAvailableAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int roomsNeeded);
        Task<int> GetAvailableRoomsCountAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut);



    }
}

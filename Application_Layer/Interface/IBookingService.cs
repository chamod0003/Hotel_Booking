using Application_Layer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Interface
{
    public interface IBookingService
    {
        //create booking
        Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto);


        //get booking
        Task<BookingResponseDto> GetBookingByIdAsync(int bookingId);
        Task<BookingResponseDto> GetBookingByReferenceAsync(string bookingReference);

        Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
        Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId);

        Task<IEnumerable<BookingResponseDto>> GetBookingsByHotelIdAsync(int hotelId);


        Task<IEnumerable<BookingResponseDto>> GetUpcomingBookingsAsync();






        Task<BookingResponseDto> UpdateBookingAsync(int bookingId, CreateBookingDto dto);
        Task<bool> ConfirmBookingAsync(int bookingId);
        Task<bool> UpdatePaymentAsync(int bookingId, string paymentStatus, string transactionId);

        // Cancel Booking
        Task<bool> CancelBookingAsync(int bookingId, string reason);

        // Check Availability
        Task<bool> CheckRoomAvailabilityAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int roomsNeeded);
        Task<int> GetAvailableRoomsAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut);

        // Calculate Price
        Task<decimal> CalculateBookingPriceAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int numberOfRooms);
    }



}

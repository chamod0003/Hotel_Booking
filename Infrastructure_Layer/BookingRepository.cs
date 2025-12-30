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
    public class BookingRepository:IBookingRepository
    {
        private readonly AppDbContext appDbContext;

        public BookingRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<bool> CancelBookingAsync(int BookingId, string reason)
        {
            var result = await appDbContext.Bookings.FindAsync(BookingId);
            if (result == null)
            {
                return false;
            }
            result.IsCancelled = true;
            result.CancellationDate = DateTime.UtcNow;
            result.CancellationReason = reason;
            result.BookingStatus = "Cancelled";
            result.UpdatedAt = DateTime.UtcNow;

            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            var result = await appDbContext.Bookings.AddAsync(booking);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteBookingAsync(int BookingId)
        {
            var result = await appDbContext.Bookings.FindAsync(BookingId);
            if (result == null)
            {
                return false;
            }
            appDbContext.Bookings.Remove(result);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingAsync()
        {
            
            return await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                .Include(b => b.RoomType)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<int> GetAvailableRoomsCountAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            // 1. Get total rooms of this type
            var room = await appDbContext.HotelRooms
                .FirstOrDefaultAsync(r => r.HotelId == hotelId && r.RoomTypeId == roomTypeId);

            if (room == null) return 0;

            var totalRooms = room.TotalRooms;

            // 2. Calculate booked rooms for the date range
            // Booking overlaps if: (checkIn < booking.CheckOut AND checkOut > booking.CheckIn)
            var bookedRooms = await appDbContext.Bookings
                .Where(b => b.HotelId == hotelId &&
                           b.RoomTypeId == roomTypeId &&
                           !b.IsCancelled &&
                           b.BookingStatus != "Cancelled" &&
                           b.CheckInDate < checkOut &&
                           b.CheckOutDate > checkIn)
                .SumAsync(b => b.NumberOfRooms);

            // 3. Available = Total - Booked
            var available = totalRooms - bookedRooms;

            Console.WriteLine($" Hotel:{hotelId} Room:{roomTypeId} | Total:{totalRooms} Booked:{bookedRooms} Available:{available}");

            return available;
        }


        public async Task<IEnumerable<Booking>> GetBookingByUserIdAsync(Guid UserId)
        {
            var reult = await appDbContext.Bookings

                .Include(b => b.User)
                .Include(b => b.Hotel)
                    .ThenInclude(h => h.Pictures)
                .Include(b => b.RoomType)
                .Where(b => b.UserId == UserId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return reult;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByHotelIdAsync(int HotelId)
        {
            var result = await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                .Include(b => b.RoomType)
                .Where(b => b.HotelId == HotelId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return result;


        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string Status)
        {
            var result = await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                .Include(b => b.RoomType)
                .Where(b => b.BookingStatus == Status)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return result;

        }

        public async Task<Booking> GetByIdBookingAsync(int BookingId)
        {
            var result = await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                    .ThenInclude(h => h.Pictures)
                .Include(b => b.RoomType)
                .FirstOrDefaultAsync(b => b.BookingId == BookingId);
            return result;
        }

        public async Task<Booking> GetByReferncesAsync(string BookingRefernces)
        {
            var result = await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                .Include(b => b.RoomType)
                .FirstOrDefaultAsync(b => b.BookingReference == BookingRefernces);
            return result;


        }

        public async Task<IEnumerable<Booking>> GetUpcomingBookingsAsync()
        {
            var today = DateTime.Today;
            return await appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.Hotel)
                .Include(b => b.RoomType)
                .Where(b => b.CheckInDate >= today &&
                           b.BookingStatus == "Confirmed" &&
                           !b.IsCancelled)
                .OrderBy(b => b.CheckInDate)
                .ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int roomsNeeded)
        {
            var result = await GetAvailableRoomsCountAsync(hotelId, roomTypeId, checkIn, checkOut);
            return result >= roomsNeeded;
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            booking.UpdatedAt = DateTime.UtcNow;
            appDbContext.Bookings.Update(booking);
            await appDbContext.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> UpdateBookingStatusAsync(int BookingId, string Status)
        {
            var result = await appDbContext.Bookings.FindAsync(BookingId);
            if (result == null)
            {
                return false;
            }
            result.BookingStatus = Status;
            result.UpdatedAt = DateTime.UtcNow;
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePaymentStatusAsync(int BookingId, string paymentStatus, string transactionId)
        {
            var result = await appDbContext.Bookings.FindAsync(BookingId);
            if (result == null)
            {
                return false;
            }
            result.PaymentStatus = paymentStatus;
            result.PaymentTransactionId = transactionId;
            result.PaymentDate = DateTime.UtcNow;
            result.UpdatedAt = DateTime.UtcNow;

            if (paymentStatus == "Paid")
            {
                result.BookingStatus = "Confirmed";
            }
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DTO
{
    public class CreateBookingDto
    {
        [Required]
        public Guid UserId { get; set; } // From logged-in user

        [Required]
        public int HotelId { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumberOfAdults { get; set; }

        [Range(0, 10)]
        public int NumberOfChildren { get; set; }

        [Required]
        [Range(1, 5)]
        public int NumberOfRooms { get; set; }

        [StringLength(1000)]
        public string? SpecialRequests { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        // Optional: User can update their contact info during booking
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    // Booking Response DTO
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalNights { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfRooms { get; set; }
        public int TotalGuests { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string? SpecialRequests { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsCancelled { get; set; }
    }

    // User's Booking List DTO
    public class UserBookingListDto
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; }
        public string HotelName { get; set; }
        public string RoomTypeName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime BookingDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }


        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }


        [Required]
        [ForeignKey("HotelId")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }


        [Required]
        [ForeignKey("RoomTypeId")]
        public int RoomTypeId { get; set; }

        public RoomType RoomType { get; set; }

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

        [NotMapped]
        public int TotalGuests => NumberOfAdults + NumberOfChildren;

        [NotMapped]
        public int TotalNights => (CheckOutDate - CheckInDate).Days;

        // PRICING INFORMATION
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerNight { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SubTotal { get; set; } // PricePerNight * TotalNights * NumberOfRooms

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // SubTotal - Discount + Tax

        // PAYMENT INFORMATION
        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed, Refunded

        [StringLength(50)]
        public string PaymentMethod { get; set; } // CreditCard, DebitCard, Cash, BankTransfer

        [StringLength(100)]
        public string? PaymentTransactionId { get; set; }

        public DateTime? PaymentDate { get; set; }

        // BOOKING STATUS
        [Required]
        [StringLength(50)]
        public string BookingStatus { get; set; } = "Pending"; // Confirmed, Pending, Cancelled, Completed, NoShow

        // SPECIAL REQUESTS
        [StringLength(1000)]
        public string? SpecialRequests { get; set; }

        // CANCELLATION
        public bool IsCancelled { get; set; } = false;
        public DateTime? CancellationDate { get; set; }

        [StringLength(500)]
        public string? CancellationReason { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundAmount { get; set; }

        // TIMESTAMPS
        [Required]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // CONFIRMATION REFERENCE
        [Required]
        [StringLength(50)]
        public string BookingReference { get; set; }







    }
}

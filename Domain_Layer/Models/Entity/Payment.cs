using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class Payment
    {

        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; }

        [StringLength(500)]
        public string? PaymentGatewayResponse { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "LKR";
        public string BookingReference { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        // Card details
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }

        // Bank transfer
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }

    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public DateTime ProcessedAt { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }
    }
}

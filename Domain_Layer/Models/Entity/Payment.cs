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
}

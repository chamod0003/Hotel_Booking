using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string? ProfilePicture { get; set; }

        [MaxLength(50)]
        public string? OAuthProvider { get; set; } 

        [MaxLength(255)]
        public string? OAuthId { get; set; }

    }
}

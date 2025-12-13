using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class RoomType
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public string RoomTypeName { get; set; }

        public ICollection<HotelRoom> HotelRooms { get; set; }
    }
}

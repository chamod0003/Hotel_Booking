using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class HotelAmenity
    {
        [Key]
        public int HotelAmenityId { get; set; }
        public int HotelId { get; set; }
        public int AmenityTypeId { get; set; }
        public AmenityType AmenityType { get; set; }
        public bool IsAvailable { get; set; }
        public string AdditionalInfo { get; set; }

        // Navigation Property
        public Hotel Hotel { get; set; }
    }
}

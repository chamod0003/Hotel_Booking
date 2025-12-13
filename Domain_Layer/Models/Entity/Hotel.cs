using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string BriefDescription { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? DistrictId { get; set; }
        public District District { get; set; }

        public ICollection<HotelPicture> Pictures { get; set; }
        public ICollection<HotelPhoneNumber> PhoneNumbers { get; set; }
        public ICollection<HotelRoom> Rooms { get; set; }
        public ICollection<HotelAmenity> Amenities { get; set; }
    }
}

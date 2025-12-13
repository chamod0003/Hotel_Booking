using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class HotelPhoneNumber
    {
        public int HotelPhoneNumberId { get; set; }
        public int HotelId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }
        public bool IsPrimary { get; set; }
        public Hotel Hotel { get; set; }
    }
}

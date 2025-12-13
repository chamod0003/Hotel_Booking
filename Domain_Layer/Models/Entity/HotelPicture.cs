using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class HotelPicture
    {
        [Key]
        public int HotelPictureId { get; set; }
        public int HotelId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMainPicture { get; set; }
        public int DisplayOrder { get; set; }
        public string Caption { get; set; }

      
        public Hotel Hotel { get; set; }
    }
}

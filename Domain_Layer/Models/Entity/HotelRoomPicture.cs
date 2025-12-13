using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Models.Entity
{
    public class HotelRoomPicture
    {
        [Key]
        public int HotelRoomPictureId { get; set; }
        public int HotelRoomId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMainPicture { get; set; }
        public int DisplayOrder { get; set; }
        public string Caption { get; set; }

        public HotelRoom HotelRoom { get; set; }
    }
}

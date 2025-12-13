using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Models.Entity
{
    public class HotelRoom
    {
        [Key]
        public int HotelRoomId { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public int TotalRooms { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal FinalDiscountedPrice { get; set; }
        public int MaxOccupancy { get; set; }
        public int BedCount { get; set; }
        public decimal RoomSize { get; set; } 
        public string Description { get; set; }

        public decimal DiscountedPrice => DiscountPercentage > 0 
            ? PricePerNight - (PricePerNight * DiscountPercentage / 100) 
            : PricePerNight;

        public Hotel Hotel { get; set; }
        public ICollection<HotelRoomPicture> Pictures { get; set; }
    }
}

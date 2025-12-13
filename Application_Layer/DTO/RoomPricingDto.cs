using Domain_Layer.Models.Entity;

namespace Application_Layer.DTO
{
    public class RoomPricingDto
    {
        public int HotelRoomId { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public RoomType RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalSavings { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalOriginalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
    }
}

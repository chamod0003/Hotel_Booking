namespace Application_Layer.DTO
{
    public class UpdateHotelDto
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string BriefDescription { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public int? DistrictId { get; set; }
        public List<CreateHotelPictureDto> Pictures { get; set; }
        public List<CreateHotelPhoneNumberDto> PhoneNumbers { get; set; }
        public List<CreateHotelRoomDto> Rooms { get; set; }
        public List<CreateHotelAmenityDto> Amenities { get; set; }
    }
}

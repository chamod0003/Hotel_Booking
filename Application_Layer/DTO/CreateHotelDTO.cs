using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application_Layer.DTO
{
   
        public class CreateHotelDto
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
        
       

    public class CreateHotelPictureDto
        {
            public string ImageUrl { get; set; }
            public bool IsMainPicture { get; set; }
            public int DisplayOrder { get; set; }
            public string Caption { get; set; }
        }

        public class CreateHotelPhoneNumberDto
        {
            public string PhoneNumber { get; set; }
            public string PhoneType { get; set; }
            public bool IsPrimary { get; set; }
        }

        public class CreateHotelRoomDto
        {
            public int RoomTypeId { get; set; }
            public int TotalRooms { get; set; }
            public decimal PricePerNight { get; set; }

            public decimal DiscountPercentage { get; set; } = 0;
            public decimal FinalDiscountedPrice { get; set; }
            public int MaxOccupancy { get; set; }
            public int BedCount { get; set; }
            public decimal RoomSize { get; set; }
            public string Description { get; set; }
            public List<CreateHotelRoomPictureDto> Pictures { get; set; }
        }

        public class CreateHotelRoomPictureDto
        {
            public string ImageUrl { get; set; }
            public bool IsMainPicture { get; set; }
            public int DisplayOrder { get; set; }
            public string Caption { get; set; }
        }

        public class CreateHotelAmenityDto
        {
            public int AmenityTypeId { get; set; }
            public bool IsAvailable { get; set; }
            public string AdditionalInfo { get; set; }
        }
    
}

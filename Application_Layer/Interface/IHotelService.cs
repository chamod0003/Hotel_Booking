using Application_Layer.DTO;
using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Interface
{
    public interface IHotelService
    {
        Task<CreateHotelDto> GetHotelByIdAsync(int hotelId);
        Task<IEnumerable<CreateHotelDto>> GetAllHotelsAsync();
        Task<CreateHotelDto> CreateHotelAsync(CreateHotelDto createHotelDto);
        Task<UpdateHotelDto> UpdateHotelAsync(UpdateHotelDto updateHotelDto);
        Task<bool> DeleteHotelAsync(int hotelId);
        Task<IEnumerable<CreateHotelDto>> SearchHotelsByNameAsync(string hotelName);
        Task<IEnumerable<CreateHotelDto>> GetNearbyHotelsAsync(decimal latitude, decimal longitude, decimal radiusKm);
        Task<RoomPricingDto> CalculateRoomPricingAsync(int hotelId, int roomType, int numberOfNights);
        Task<IEnumerable<RoomType>> GetRoomTypesAsync();
        Task<IEnumerable<AmenityType>> GetAmenityTypesAsync();
        Task<IEnumerable<District>> GetDistrictsAsync();
    }

}

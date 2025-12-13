using Domain_Layer.Models.Entity;

namespace Domain_Layer.Interface
{
    public interface IHotelRepository
    {
        Task<Hotel> AddAsync(Hotel hotel);  //commond

        Task<Hotel> GetByIdAsync(int hotelId); //Query

        Task<IList<Hotel>> GetAllAsync(); //Query

        Task<Hotel> UpdateAsync(Hotel hotel);  //commond

        Task<bool> DeleteAsync(int hotelId);  //commond

        Task<IList<Hotel>> SearchByNameAsync(string hotelname); //Query
        Task<IList<Hotel>> GetHotelsByLocationAsync(decimal latitude, decimal longitude, decimal radiusKm); //Query
        Task<IList<Hotel>> GetHotelsByAmenityAsync(AmenityType amenityType); //Query
        Task<IList<Hotel>> GetHotelsByRoomTypeAsync(RoomType roomType); //Query
        Task<IList<Hotel>> GetHotelsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IList<RoomType>> GetAllRoomTypesAsync();
        Task<IList<AmenityType>> GetAllAmenityTypesAsync();
        Task<IList<District>> GetAllDistrictsAsync();


    }
}

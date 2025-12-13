using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Infrastructure_Layer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer
{
    public class HotelRepository:IHotelRepository
    {
        private readonly AppDbContext appdbcontext;

        public HotelRepository(AppDbContext appDbContext)
        {
            this.appdbcontext = appDbContext;
        }

        public async Task<Hotel> AddAsync(Hotel hotel)
        {
            var result = await appdbcontext.Hotels.AddAsync(hotel);
            await appdbcontext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int hotelId)
        {
            var result = await appdbcontext.Hotels.FindAsync(hotelId);
            if (result == null) return false;

            appdbcontext.Hotels.Remove(result);
            await appdbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<IList<Hotel>> GetAllAsync()
        {
            var result = await appdbcontext.Hotels
                            .Include(h => h.Pictures)
                            .Include(h => h.PhoneNumbers)
                            .Include(h => h.Rooms)
                            .Include(h => h.Amenities)
                            .ToListAsync();
            return result;
        }

        public async Task<Hotel> GetByIdAsync(int hotelId)
        {
            var result = await appdbcontext.Hotels
                                 .Include(h => h.Pictures)
                                 .Include(h => h.PhoneNumbers)
                                 .Include(h => h.Rooms)
                                 .Include(h => h.Amenities)
                                 .FirstOrDefaultAsync(h => h.HotelId == hotelId);
            return result;
        }

        public async Task<IList<Hotel>> GetHotelsByAmenityAsync(AmenityType amenityType)
        {
            var hotels = await appdbcontext.Hotels
                .Include(h => h.Amenities)
                .Where(h => h.Amenities.Any(a => a.AmenityType.AmenityTypeId == amenityType.AmenityTypeId && a.IsAvailable))
                .ToListAsync();
            return hotels;
        }

        public async Task<IList<Hotel>> GetHotelsByLocationAsync(decimal latitude, decimal longitude, decimal radiusKm)
        {
            // Haversine formula for distance calculation
            double ToRadians(double angle) => Math.PI * angle / 180.0;
            var hotels = await appdbcontext.Hotels
                            .Include(h => h.Pictures)
                            .Include(h => h.PhoneNumbers)
                            .Include(h => h.Rooms)
                            .Include(h => h.Amenities)
                            .ToListAsync();
            var nearbyHotels = hotels.Where(h =>
            {
                var dLat = ToRadians((double)(h.Latitude - latitude));
                var dLon = ToRadians((double)(h.Longitude - longitude));
                var lat1 = ToRadians((double)latitude);
                var lat2 = ToRadians((double)h.Latitude);
                var a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var distance = 6371 * c; // Earth radius in km
                return distance <= (double)radiusKm;
            }).ToList();
            return nearbyHotels;
        }

        public async Task<IList<Hotel>> GetHotelsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var hotels = await appdbcontext.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.Rooms.Any(r => r.PricePerNight >= minPrice && r.PricePerNight <= maxPrice))
                .ToListAsync();
            return hotels;
        }

        public async Task<IList<Hotel>> GetHotelsByRoomTypeAsync(RoomType roomType)
        {
            var hotels = await appdbcontext.Hotels
                .Include(h => h.Rooms)
                .Where(h => h.Rooms.Any(r => r.RoomType.RoomId == roomType.RoomId))
                .ToListAsync();
            return hotels;
        }

        public async Task<IList<Hotel>> SearchByNameAsync(string hotelname)
        {
            var hotels = await appdbcontext.Hotels
                .Include(h => h.Pictures)
                .Include(h => h.PhoneNumbers)
                .Include(h => h.Rooms)
                .Include(h => h.Amenities)
                .Where(h => h.HotelName.Contains(hotelname))
                .ToListAsync();
            return hotels;
        }

        public async Task<Hotel> UpdateAsync(Hotel hotel)
        {
            var existingHotel = await appdbcontext.Hotels
                .Include(h => h.Pictures)
                .Include(h => h.PhoneNumbers)
                .Include(h => h.Rooms)
                .Include(h => h.Amenities)
                .FirstOrDefaultAsync(h => h.HotelId == hotel.HotelId);
            if (existingHotel == null) return null;

            appdbcontext.Entry(existingHotel).CurrentValues.SetValues(hotel);
            await appdbcontext.SaveChangesAsync();
            return existingHotel;
        }

        public async Task<IList<RoomType>> GetAllRoomTypesAsync()
        {
            return await appdbcontext.Set<RoomType>().ToListAsync();
        }

        public async Task<IList<AmenityType>> GetAllAmenityTypesAsync()
        {
            return await appdbcontext.Set<AmenityType>().ToListAsync();
        }

        public async Task<IList<District>> GetAllDistrictsAsync()
        {
            return await appdbcontext.Set<District>().ToListAsync();
        }
    }
}

using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application_Layer
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository hotelRepository;

        private readonly IMemoryCache memoryCache;
        private IHotelRepository @object;

        public HotelService(IHotelRepository hotelRepository, IMemoryCache memoryCache)
        {
            this.hotelRepository = hotelRepository;
            this.memoryCache = memoryCache;
        }

        public HotelService(IHotelRepository @object)
        {
            this.@object = @object;
        }

        public async Task<CreateHotelDto> CreateHotelAsync(CreateHotelDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.HotelName))
                throw new Exception("Hotel Name is required.");

            var hotel = new Hotel
            {
                HotelName = dto.HotelName,
                
                BriefDescription = dto.BriefDescription,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                DistrictId = dto.DistrictId,
                Pictures = dto.Pictures?.Select(p => new HotelPicture
                {
                    ImageUrl = p.ImageUrl,
                    IsMainPicture = p.IsMainPicture,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                }).ToList() ?? new List<HotelPicture>(),

                PhoneNumbers = dto.PhoneNumbers?.Select(p => new HotelPhoneNumber
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneType = p.PhoneType,
                    IsPrimary = p.IsPrimary
                }).ToList() ?? new List<HotelPhoneNumber>(),

                Rooms = dto.Rooms?.Select(r => new HotelRoom
                {
                    RoomTypeId = r.RoomTypeId,
                    TotalRooms = r.TotalRooms,
                    PricePerNight = r.PricePerNight,
                    DiscountPercentage = r.DiscountPercentage,
                    FinalDiscountedPrice = r.DiscountPercentage > 0 
                        ? r.PricePerNight - (r.PricePerNight * r.DiscountPercentage / 100) 
                        : r.PricePerNight,
                    MaxOccupancy = r.MaxOccupancy,
                    BedCount = r.BedCount,
                    RoomSize = r.RoomSize,
                    Description = r.Description,
                    Pictures = r.Pictures?.Select(p => new HotelRoomPicture
                    {
                        ImageUrl = p.ImageUrl,
                        IsMainPicture = p.IsMainPicture,
                        DisplayOrder = p.DisplayOrder,
                        Caption = p.Caption
                    }).ToList() ?? new List<HotelRoomPicture>()
                }).ToList() ?? new List<HotelRoom>(),

                Amenities = dto.Amenities?.Select(a => new HotelAmenity
                {
                    AmenityTypeId = a.AmenityTypeId,
                    IsAvailable = a.IsAvailable,
                    AdditionalInfo = a.AdditionalInfo
                }).ToList() ?? new List<HotelAmenity>()
            };

            await hotelRepository.AddAsync(hotel);
            
            // Update the DTO with the generated HotelId
            dto.HotelId = hotel.HotelId;

            return dto;
        }

        // ----------------- GET ALL HOTELS --------------------
        public async Task<IEnumerable<CreateHotelDto>> GetAllHotelsAsync()
        {
            var cachekey = $"all_hotels";

            if(memoryCache.TryGetValue(cachekey, out IEnumerable<CreateHotelDto> cachedHotels))
            {
                return cachedHotels;
            }

            var hotels = await hotelRepository.GetAllAsync();
            var hotelsDto= hotels.Select(h => new CreateHotelDto
            {
                HotelId = h.HotelId,
                HotelName = h.HotelName,
                BriefDescription = h.BriefDescription,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                Address = h.Address,
                DistrictId = h.DistrictId,
                Pictures = h.Pictures?.Select(p => new CreateHotelPictureDto
                {
                    ImageUrl = p.ImageUrl,
                    IsMainPicture = p.IsMainPicture,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                }).ToList() ?? new List<CreateHotelPictureDto>(),

                PhoneNumbers = h.PhoneNumbers?.Select(p => new CreateHotelPhoneNumberDto
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneType = p.PhoneType,
                    IsPrimary = p.IsPrimary
                }).ToList() ?? new List<CreateHotelPhoneNumberDto>(),

                Rooms = h.Rooms?.Select(r => new CreateHotelRoomDto
                {
                    RoomTypeId = r.RoomTypeId,
                    TotalRooms = r.TotalRooms,
                    PricePerNight = r.PricePerNight,
                    DiscountPercentage = r.DiscountPercentage,
                    FinalDiscountedPrice = r.FinalDiscountedPrice,
                    MaxOccupancy = r.MaxOccupancy,
                    BedCount = r.BedCount,
                    RoomSize = r.RoomSize,
                    Description = r.Description,
                    Pictures = r.Pictures?.Select(p => new CreateHotelRoomPictureDto
                    {
                        ImageUrl = p.ImageUrl,
                        IsMainPicture = p.IsMainPicture,
                        DisplayOrder = p.DisplayOrder,
                        Caption = p.Caption
                    }).ToList() ?? new List<CreateHotelRoomPictureDto>()
                }).ToList() ?? new List<CreateHotelRoomDto>(),

                Amenities = h.Amenities?.Select(a => new CreateHotelAmenityDto
                {
                    AmenityTypeId = a.AmenityType?.AmenityTypeId ?? 0,
                    IsAvailable = a.IsAvailable,
                    AdditionalInfo = a.AdditionalInfo
                }).ToList() ?? new List<CreateHotelAmenityDto>()
            }).ToList();

            memoryCache.Set(cachekey, hotelsDto, TimeSpan.FromMinutes(10));
            return hotelsDto;
        }

        // ----------------- GET HOTEL BY ID --------------------
        public async Task<CreateHotelDto> GetHotelByIdAsync(int hotelId)
        {
            var cachekey = $"hotel_{hotelId}";

            if (memoryCache.TryGetValue(cachekey, out CreateHotelDto cachedHotels))
            {
                return cachedHotels;
            }

            var hotel = await hotelRepository.GetByIdAsync(hotelId);

            if (hotel == null)
                return null;

            var hotelDto =  new CreateHotelDto
            {
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                BriefDescription = hotel.BriefDescription,
                Latitude = hotel.Latitude,
                Longitude = hotel.Longitude,
                Address = hotel.Address,
                DistrictId = hotel.DistrictId,
                Pictures = hotel.Pictures?.Select(p => new CreateHotelPictureDto
                {
                    ImageUrl = p.ImageUrl,
                    IsMainPicture = p.IsMainPicture,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                }).ToList() ?? new List<CreateHotelPictureDto>(),

                PhoneNumbers = hotel.PhoneNumbers?.Select(p => new CreateHotelPhoneNumberDto
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneType = p.PhoneType,
                    IsPrimary = p.IsPrimary
                }).ToList() ?? new List<CreateHotelPhoneNumberDto>(),

                Rooms = hotel.Rooms?.Select(r => new CreateHotelRoomDto
                {
                    RoomTypeId = r.RoomTypeId,
                    TotalRooms = r.TotalRooms,
                    PricePerNight = r.PricePerNight,
                    DiscountPercentage = r.DiscountPercentage,
                    FinalDiscountedPrice = r.FinalDiscountedPrice,
                    MaxOccupancy = r.MaxOccupancy,
                    BedCount = r.BedCount,
                    RoomSize = r.RoomSize,
                    Description = r.Description,
                    Pictures = r.Pictures?.Select(p => new CreateHotelRoomPictureDto
                    {
                        ImageUrl = p.ImageUrl,
                        IsMainPicture = p.IsMainPicture,
                        DisplayOrder = p.DisplayOrder,
                        Caption = p.Caption
                    }).ToList() ?? new List<CreateHotelRoomPictureDto>()
                }).ToList() ?? new List<CreateHotelRoomDto>(),

                Amenities = hotel.Amenities?.Select(a => new CreateHotelAmenityDto
                {
                    AmenityTypeId = a.AmenityType?.AmenityTypeId ?? 0,
                    IsAvailable = a.IsAvailable,
                    AdditionalInfo = a.AdditionalInfo
                }).ToList() ?? new List<CreateHotelAmenityDto>()
            };

            memoryCache.Set(cachekey, hotelDto, TimeSpan.FromMinutes(10));
            return hotelDto;
        }

        // ----------------- DELETE HOTEL --------------------
        public async Task<bool> DeleteHotelAsync(int hotelId)
        {
            return await hotelRepository.DeleteAsync(hotelId);
        }

        // ----------------- SEARCH BY NAME --------------------
        public async Task<IEnumerable<CreateHotelDto>> SearchHotelsByNameAsync(string hotelName)
        {
            var hotels = await hotelRepository.SearchByNameAsync(hotelName);

            return hotels.Select(h => new CreateHotelDto
            {
                HotelId = h.HotelId,
                HotelName = h.HotelName,
                BriefDescription = h.BriefDescription,
                Latitude = h.Latitude,
                Longitude = h.Longitude,
                Address = h.Address,
                DistrictId = h.DistrictId
            }).ToList();
        }

        // ----------------- NEARBY HOTELS --------------------
        public async Task<IEnumerable<CreateHotelDto>> GetNearbyHotelsAsync(decimal latitude, decimal longitude, decimal radiusKm)
        {
            var hotels = await hotelRepository.GetAllAsync();

            return hotels
                .Where(h => CalculateDistance(latitude, longitude, h.Latitude, h.Longitude) <= (double)radiusKm)
                .Select(h => new CreateHotelDto
                {
                    HotelId = h.HotelId,
                    HotelName = h.HotelName,
                    BriefDescription = h.BriefDescription,
                    Latitude = h.Latitude,
                    Longitude = h.Longitude,
                    Address = h.Address,
                    DistrictId = h.DistrictId
                }).ToList();
        }

        private double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double R = 6371; // Earth radius km  

            double dLat = Math.PI / 180 * ((double)lat2 - (double)lat1);
            double dLon = Math.PI / 180 * ((double)lon2 - (double)lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos((double)lat1 * Math.PI / 180) * Math.Cos((double)lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        // ----------------- UPDATE HOTEL --------------------
        public async Task<UpdateHotelDto> UpdateHotelAsync(UpdateHotelDto dto)
        {
            var hotel = await hotelRepository.GetByIdAsync(dto.HotelId);
            if (hotel == null)
                throw new Exception("Hotel not found.");

            hotel.HotelName = dto.HotelName;
            hotel.BriefDescription = dto.BriefDescription;
            hotel.Latitude = dto.Latitude;
            hotel.Longitude = dto.Longitude;
            hotel.Address = dto.Address;
            hotel.DistrictId = dto.DistrictId;

            hotel.Pictures = dto.Pictures?.Select(p => new HotelPicture
            {
                ImageUrl = p.ImageUrl,
                IsMainPicture = p.IsMainPicture,
                DisplayOrder = p.DisplayOrder,
                Caption = p.Caption
            }).ToList() ?? new List<HotelPicture>();

            hotel.PhoneNumbers = dto.PhoneNumbers?.Select(p => new HotelPhoneNumber
            {
                PhoneNumber = p.PhoneNumber,
                PhoneType = p.PhoneType,
                IsPrimary = p.IsPrimary
            }).ToList() ?? new List<HotelPhoneNumber>();

            hotel.Rooms = dto.Rooms?.Select(r => new HotelRoom
            {
                RoomTypeId = r.RoomTypeId,
                TotalRooms = r.TotalRooms,
                PricePerNight = r.PricePerNight,
                DiscountPercentage = r.DiscountPercentage,
                FinalDiscountedPrice = r.DiscountPercentage > 0 
                    ? r.PricePerNight - (r.PricePerNight * r.DiscountPercentage / 100) 
                    : r.PricePerNight,
                MaxOccupancy = r.MaxOccupancy,
                BedCount = r.BedCount,
                RoomSize = r.RoomSize,
                Description = r.Description,
                Pictures = r.Pictures?.Select(p => new HotelRoomPicture
                {
                    ImageUrl = p.ImageUrl,
                    IsMainPicture = p.IsMainPicture,
                    DisplayOrder = p.DisplayOrder,
                    Caption = p.Caption
                }).ToList() ?? new List<HotelRoomPicture>()
            }).ToList() ?? new List<HotelRoom>();

            hotel.Amenities = dto.Amenities?.Select(a => new HotelAmenity
            {
                AmenityTypeId = a.AmenityTypeId,
                IsAvailable = a.IsAvailable,
                AdditionalInfo = a.AdditionalInfo
            }).ToList() ?? new List<HotelAmenity>();

            await hotelRepository.UpdateAsync(hotel);

            return dto;
        }

        // ----------------- CALCULATE ROOM PRICING WITH DISCOUNT --------------------
        public async Task<RoomPricingDto> CalculateRoomPricingAsync(int hotelId, int roomType, int numberOfNights)
        {
            var hotel = await hotelRepository.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new Exception("Hotel not found.");

            var room = hotel.Rooms?.FirstOrDefault(r => r.RoomTypeId == roomType);
            if (room == null)
                throw new Exception("Room type not found in this hotel.");

            var discountAmount = room.PricePerNight * room.DiscountPercentage / 100;
            var discountedPrice = room.DiscountedPrice;
            var totalOriginalPrice = room.PricePerNight * numberOfNights;
            var totalDiscountedPrice = discountedPrice * numberOfNights;
            var totalSavings = totalOriginalPrice - totalDiscountedPrice;

            return new RoomPricingDto
            {
                HotelRoomId = room.HotelRoomId,
                HotelId = hotel.HotelId,
                HotelName = hotel.HotelName,
                RoomType = room.RoomType,
                PricePerNight = room.PricePerNight,
                DiscountPercentage = room.DiscountPercentage,
                DiscountAmount = discountAmount,
                DiscountedPrice = discountedPrice,
                NumberOfNights = numberOfNights,
                TotalOriginalPrice = totalOriginalPrice,
                TotalDiscountedPrice = totalDiscountedPrice,
                TotalSavings = totalSavings
            };
        }

        public async Task<IEnumerable<RoomType>> GetRoomTypesAsync()
        {
            return await hotelRepository.GetAllRoomTypesAsync();
        }

        public async Task<IEnumerable<AmenityType>> GetAmenityTypesAsync()
        {
            return await hotelRepository.GetAllAmenityTypesAsync();
        }

        public async Task<IEnumerable<District>> GetDistrictsAsync()
        {
            return await hotelRepository.GetAllDistrictsAsync();
        }

 
      
    }
}

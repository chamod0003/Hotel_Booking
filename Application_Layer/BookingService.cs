using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Enums;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.Extensions.Caching.Memory;

namespace Application_Layer
{
    public class BookingService:IBookingService
    {
        private readonly IBookingRepository bookingRepository;

        private readonly IUserRepository userRepository;

        private readonly IHotelRepository hotelRepository;

        private readonly IMemoryCache memoryCache;

        private readonly IBookingSubject bookingSubject;

        public BookingService(IBookingSubject bookingSubject,IBookingRepository bookingRepository,IUserRepository userRepository,IHotelRepository hotelRepository,IMemoryCache memoryCache)
        {
            this.bookingSubject = bookingSubject;
            this.bookingRepository = bookingRepository;
            this.userRepository = userRepository;
            this.hotelRepository = hotelRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<decimal> CalculateBookingPriceAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int numberOfRooms)
        {
            var hotel = await hotelRepository.GetByIdAsync(hotelId);
            if (hotel == null)
                throw new Exception("Hotel not found");

            var room = hotel.Rooms.FirstOrDefault(r => r.RoomTypeId == roomTypeId);
            if (room == null)
                throw new Exception("Room type not found");

            var totalNights = (checkOut - checkIn).Days;
            var pricePerNight = room.FinalDiscountedPrice > 0 ? room.FinalDiscountedPrice : room.PricePerNight;
            var subTotal = pricePerNight * totalNights * numberOfRooms;
            var taxAmount = subTotal * 0.05m;
            var totalAmount = subTotal + taxAmount;

            return totalAmount;
        }

        public async Task<bool> CancelBookingAsync(int bookingId, string reason)
        {
            return await bookingRepository.CancelBookingAsync(bookingId, reason);
        }

        public async Task<bool> CheckRoomAvailabilityAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut, int roomsNeeded)
        {
            return await bookingRepository.IsRoomAvailableAsync(hotelId, roomTypeId, checkIn, checkOut, roomsNeeded);
        }

        public async Task<bool> ConfirmBookingAsync(int bookingId)
        {
            return await bookingRepository.UpdateBookingStatusAsync(bookingId, "Confirmed");
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {

            var user = await userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");

            var hotel = await hotelRepository.GetByIdAsync(dto.HotelId);
            if (hotel == null)
                throw new Exception("Hotel not found");

            var room = hotel.Rooms.FirstOrDefault(r => r.RoomTypeId == dto.RoomTypeId);
            if (room == null)
                throw new Exception("Room type not found");

            // 3. CHECK AVAILABILITY - CRITICAL!
            var availableRooms = await bookingRepository.GetAvailableRoomsCountAsync(
                dto.HotelId,
                dto.RoomTypeId,
                dto.CheckInDate,
                dto.CheckOutDate);

            if (availableRooms < dto.NumberOfRooms)
            {
                throw new Exception($"Only {availableRooms} room(s) available. You requested {dto.NumberOfRooms} room(s).");
            }

            // 4. Calculate pricing
            var totalNights = (dto.CheckOutDate - dto.CheckInDate).Days;
            var pricePerNight = room.FinalDiscountedPrice > 0 ? room.FinalDiscountedPrice : room.PricePerNight;
            var subTotal = pricePerNight * totalNights * dto.NumberOfRooms;
            var taxAmount = subTotal * 0.05m;
            var totalAmount = subTotal + taxAmount;

            // 5. Update user contact info if provided
            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                await userRepository.UpdateUserContactInfoAsync(
                    dto.UserId, dto.PhoneNumber, dto.Address, dto.City, dto.Country);
            }

            // 6. Create booking
            var booking = new Booking
            {
                UserId = dto.UserId,
                HotelId = dto.HotelId,
                RoomTypeId = dto.RoomTypeId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                NumberOfAdults = dto.NumberOfAdults,
                NumberOfChildren = dto.NumberOfChildren,
                NumberOfRooms = dto.NumberOfRooms, // ✅ This reduces available count!
                PricePerNight = pricePerNight,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                SpecialRequests = dto.SpecialRequests,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                BookingStatus = "Confirmed",
                BookingReference = GenerateBookingReference(),
                BookingDate = DateTime.UtcNow
            };

            var createdBooking = await bookingRepository.CreateBookingAsync(booking);

            // 7. Clear availability cache
            memoryCache.Remove($"availability:{dto.HotelId}:{dto.RoomTypeId}");
            memoryCache.Remove($"bookings:user:{dto.UserId}");
            memoryCache.Remove($"bookings:hotel:{dto.HotelId}");

            Console.WriteLine($" Booking created! {dto.NumberOfRooms} room(s) now unavailable for {dto.CheckInDate:yyyy-MM-dd} to {dto.CheckOutDate:yyyy-MM-dd}");

            return await GetBookingByIdAsync(createdBooking.BookingId);
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();    

            var cachekey = "All_Bookings";
            if(memoryCache.TryGetValue(cachekey,out IEnumerable<BookingResponseDto> cachedBookings))
            {
                stopwatch.Stop();
                Console.WriteLine($"CACHE HIT - Time: {stopwatch.ElapsedMilliseconds} ms");
                return cachedBookings;
            }   

            var bookings = await bookingRepository.GetAllBookingAsync();
            var bookingsDtos =  bookings.Select(b => MapToBookingResponseDto(b)).ToList();

            memoryCache.Set(cachekey, bookingsDtos, TimeSpan.FromMinutes(10));

            stopwatch.Stop();
            Console.WriteLine($"DB HIT - Time: {stopwatch.ElapsedMilliseconds} ms");

            return bookingsDtos;
        }

        public async Task<int> GetAvailableRoomsAsync(int hotelId, int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            return await bookingRepository.GetAvailableRoomsCountAsync(hotelId, roomTypeId, checkIn, checkOut);
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId)
        {
            var cachekey = $"Booking_{bookingId}";

            // Check cache first
            if (memoryCache.TryGetValue(cachekey, out BookingResponseDto cachedBooking))
            {
                return cachedBooking;
            }

            // If not in cache, retrieve from repository
            var booking = await bookingRepository.GetByIdBookingAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");


            var bookingDto = MapToBookingResponseDto(booking);

            //store in cache
            memoryCache.Set(cachekey, bookingDto, TimeSpan.FromMinutes(10));
            return bookingDto;
        }

        public async Task<BookingResponseDto> GetBookingByReferenceAsync(string bookingReference)
        {
            var cachekey = $"BookingRef_{bookingReference}";

            if (memoryCache.TryGetValue(cachekey, out BookingResponseDto cachedBooking))
            {
                return cachedBooking;
            }

            var booking = await bookingRepository.GetByReferncesAsync(bookingReference);
            if (booking == null)
                throw new Exception("Booking not found");

            var bookingDto = MapToBookingResponseDto(booking);

            memoryCache.Set(cachekey, bookingDto, TimeSpan.FromMinutes(10));
            return bookingDto;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByHotelIdAsync(int hotelId)
        {
            var cachekey = $"Bookings_Hotel_{hotelId}";

            // Check cache first
            if (memoryCache.TryGetValue(cachekey, out IEnumerable<BookingResponseDto> cachedBookings))
            {
                return cachedBookings;
            }

            var bookings = await bookingRepository.GetBookingsByHotelIdAsync(hotelId);
            var bookingDtos = bookings.Select(b => MapToBookingResponseDto(b));

            memoryCache.Set(cachekey, bookingDtos, TimeSpan.FromMinutes(10));
            return bookingDtos;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByUserIdAsync(Guid userId)
        {
            var cachekey = $"Bookings_User_{userId}";
            if (memoryCache.TryGetValue(cachekey, out IEnumerable<BookingResponseDto> cachedBookings))
            {
                return cachedBookings;
            }

            var bookings = await bookingRepository.GetBookingByUserIdAsync(userId);
            var bookingDtos =  bookings.Select(b => MapToBookingResponseDto(b));

            memoryCache.Set(cachekey, bookingDtos, TimeSpan.FromMinutes(10));
            return bookingDtos;
        }

        public async Task<IEnumerable<BookingResponseDto>> GetUpcomingBookingsAsync()
        {
            var bookings = await bookingRepository.GetUpcomingBookingsAsync();
            return bookings.Select(b => MapToBookingResponseDto(b));
        }

        public async Task<BookingResponseDto> UpdateBookingAsync(int bookingId, CreateBookingDto dto)
        {
            var booking = await bookingRepository.GetByIdBookingAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            var hotel = await hotelRepository.GetByIdAsync(dto.HotelId);
            if (hotel == null)
                throw new Exception("Hotel not found");

            var room = hotel.Rooms.FirstOrDefault(r => r.RoomTypeId == dto.RoomTypeId);
            if (room == null)
                throw new Exception("Room Type Not Found");

            var totalNights = (dto.CheckOutDate - dto.CheckInDate).Days;
            var pricePerNight = room.FinalDiscountedPrice > 0 ? room.FinalDiscountedPrice : room.PricePerNight;
            var subTotal = pricePerNight * totalNights * dto.NumberOfRooms;
            var taxAmount = subTotal * 0.05m;
            var totalAmount = subTotal + taxAmount;

            booking.HotelId = dto.HotelId;
            booking.RoomTypeId = dto.RoomTypeId;
            booking.CheckInDate = dto.CheckInDate;
            booking.CheckOutDate = dto.CheckOutDate;
            booking.NumberOfAdults = dto.NumberOfAdults;
            booking.NumberOfChildren = dto.NumberOfChildren;
            booking.NumberOfRooms = dto.NumberOfRooms;
            booking.PricePerNight = pricePerNight;
            booking.SubTotal = subTotal;
            booking.TaxAmount = taxAmount;
            booking.TotalAmount = totalAmount;
            booking.SpecialRequests = dto.SpecialRequests;
            booking.PaymentMethod = dto.PaymentMethod;

            var updatedBooking = await bookingRepository.UpdateBookingAsync(booking);
            return await GetBookingByIdAsync(updatedBooking.BookingId);
        }

        public async Task<bool> UpdatePaymentAsync(int bookingId, string paymentStatus, string transactionId)
        {
            return await bookingRepository.UpdatePaymentStatusAsync(bookingId, paymentStatus, transactionId);
        }


        private string GenerateBookingReference()
        {
            var year = DateTime.Now.Year;
            var random = new Random().Next(100000, 999999);
            return $"BK-{year}-{random}";
        }

        private BookingResponseDto MapToBookingResponseDto(Booking b)
        {
            return new BookingResponseDto
            {
                BookingId = b.BookingId,
                BookingReference = b.BookingReference,
                UserId = b.UserId,
                UserFullName = b.User?.FullName ?? "",
                UserEmail = b.User?.Email ?? "",
                UserPhone = b.User?.PhoneNumber ?? "",
                HotelId = b.HotelId,
                HotelName = b.Hotel?.HotelName ?? "",
                HotelAddress = b.Hotel?.Address ?? "",
                RoomTypeId = b.RoomTypeId,
                RoomTypeName = b.RoomType?.RoomTypeName ?? "",
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalNights = b.TotalNights,
                NumberOfAdults = b.NumberOfAdults,
                NumberOfChildren = b.NumberOfChildren,
                NumberOfRooms = b.NumberOfRooms,
                TotalGuests = b.TotalGuests,
                PricePerNight = b.PricePerNight,
                SubTotal = b.SubTotal,
                DiscountAmount = b.DiscountAmount,
                TaxAmount = b.TaxAmount,
                TotalAmount = b.TotalAmount,
                BookingStatus = b.BookingStatus,
                PaymentStatus = b.PaymentStatus,
                PaymentMethod = b.PaymentMethod ?? "",
                SpecialRequests = b.SpecialRequests,
                BookingDate = b.BookingDate,
                IsCancelled = b.IsCancelled
            };
        }
    }
}

using Application_Layer;
using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var booking = await bookingService.CreateBookingAsync(dto);
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await bookingService.GetBookingByIdAsync(id);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("reference/{reference}")]
        public async Task<IActionResult> GetBookingByReference(string reference)
        {
            try
            {
                var booking = await bookingService.GetBookingByReferenceAsync(reference);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(Guid userId)
        {
            var bookings = await bookingService.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetBookingsByHotelId(int hotelId)
        {
            var bookings = await bookingService.GetBookingsByHotelIdAsync(hotelId);
            return Ok(bookings);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingBookings()
        {
            var bookings = await bookingService.GetUpcomingBookingsAsync();
            return Ok(bookings);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] CreateBookingDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var booking = await bookingService.UpdateBookingAsync(id, dto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            try
            {
                var result = await bookingService.ConfirmBookingAsync(id);
                if (result)
                    return Ok(new { message = "Booking confirmed successfully" });
                return BadRequest(new { message = "Failed to confirm booking" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
        {
            try
            {
                var result = await bookingService.CancelBookingAsync(id, request.Reason);
                if (result)
                    return Ok(new { message = "Booking cancelled successfully" });
                return BadRequest(new { message = "Failed to cancel booking" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentRequest request)
        {
            try
            {
                var result = await bookingService.UpdatePaymentAsync(id, request.PaymentStatus, request.TransactionId);
                if (result)
                    return Ok(new { message = "Payment updated successfully" });
                return BadRequest(new { message = "Failed to update payment" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check-availability")]
        public async Task<ActionResult<RoomAvailabilityDto>> CheckRoomAvailability(
    [FromQuery] int hotelId,
    [FromQuery] int roomTypeId,
    [FromQuery] DateTime checkInDate,
    [FromQuery] DateTime checkOutDate,
    [FromQuery] int roomsNeeded = 1)
        {
            try
            {
                // Get available count
                var availableCount = await bookingService.GetAvailableRoomsAsync(
                    hotelId, roomTypeId, checkInDate, checkOutDate);

                var isAvailable = availableCount >= roomsNeeded;
                var totalNights = (checkOutDate - checkInDate).Days;

                return Ok(new RoomAvailabilityDto
                {
                    HotelId = hotelId,
                    RoomTypeId = roomTypeId,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    TotalNights = totalNights,
                    RequestedRooms = roomsNeeded,
                    AvailableRooms = availableCount,
                    IsAvailable = isAvailable,
                    Message = isAvailable
                        ? $"✅ {availableCount} room(s) available"
                        : $"❌ Only {availableCount} room(s) available. You requested {roomsNeeded}."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("calculate-price")]
        public async Task<IActionResult> CalculateBookingPrice(
            [FromQuery] int hotelId,
            [FromQuery] int roomTypeId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut,
            [FromQuery] int numberOfRooms)
        {
            try
            {
                var totalPrice = await bookingService.CalculateBookingPriceAsync(hotelId, roomTypeId, checkIn, checkOut, numberOfRooms);
                return Ok(new { totalPrice = totalPrice });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CancelBookingRequest
    {
        public string Reason { get; set; }
    }

    public class UpdatePaymentRequest
    {
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
    }
}

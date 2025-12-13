using Application_Layer.DTO;
using Application_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService hotelService;

        private readonly ILogger<HotelController> logger;

        public HotelController(IHotelService hotelService, ILogger<HotelController> logger)
        {
            this.hotelService = hotelService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await hotelService.GetHotelByIdAsync(id);
            if (hotel == null) return NotFound();
            return Ok(hotel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var hotel = await hotelService.CreateHotelAsync(dto);
            return CreatedAtAction(nameof(GetHotelById), new { id = hotel.HotelId }, hotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto dto)
        {
            if (id != dto.HotelId) return BadRequest("Hotel ID mismatch.");
            var updated = await hotelService.UpdateHotelAsync(dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var deleted = await hotelService.DeleteHotelAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchHotelsByName([FromQuery] string name)
        {
            var hotels = await hotelService.SearchHotelsByNameAsync(name);
            return Ok(hotels);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyHotels([FromQuery] decimal lat, [FromQuery] decimal lng, [FromQuery] decimal radius)
        {
            var hotels = await hotelService.GetNearbyHotelsAsync(lat, lng, radius);
            return Ok(hotels);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEnums()
        {
            var roomTypes = await hotelService.GetRoomTypesAsync();
            var amenityTypes = await hotelService.GetAmenityTypesAsync();
            var districts = await hotelService.GetDistrictsAsync();

            return Ok(new
            {
                roomTypes = roomTypes.Select(rt => rt.RoomTypeName),
                amenityTypes = amenityTypes.Select(at => at.Name),
                districts = districts.Select(d => d.Name),
                phoneTypes = new[] { "Mobile", "Landline", "Fax" }
            });
        }

        // Get room types only
        [HttpGet("room-types")]
        public async Task<IActionResult> GetRoomTypes()
        {
            var roomTypes = await hotelService.GetRoomTypesAsync();
            return Ok(roomTypes);
        }

        // Get amenity types only
        [HttpGet("amenity-types")]
        public async Task<IActionResult> GetAmenityTypes()
        {
            var amenityTypes = await hotelService.GetAmenityTypesAsync();
            return Ok(amenityTypes);
        }

        // Get districts only
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts()
        {
            var districts = await hotelService.GetDistrictsAsync();
            return Ok(districts);
        }

        // Get all enums with their numeric values
        [HttpGet("all-with-values")]
        public async Task<IActionResult> GetAllEnumsWithValues()
        {
            var roomTypes = await hotelService.GetRoomTypesAsync();
            var amenityTypes = await hotelService.GetAmenityTypesAsync();
            var districts = await hotelService.GetDistrictsAsync();

            return Ok(new
            {
                roomTypes = roomTypes.Select(rt => new { Value = rt.RoomId, Name = rt.RoomTypeName }),
                amenityTypes = amenityTypes.Select(at => new { Value = at.AmenityTypeId, Name = at.Name }),
                districts = districts.Select(d => new { Value = d.DistrictId, Name = d.Name }),
                phoneTypes = new[]
                {
                    new { Value = 1, Name = "Mobile" },
                    new { Value = 2, Name = "Landline" },
                    new { Value = 3, Name = "Fax" }
                }
            });
        }

        [HttpGet("{hotelId}/room-pricing")]
        public async Task<IActionResult> CalculateRoomPricing(int hotelId, [FromQuery] int roomType, [FromQuery] int numberOfNights = 1)
        {
            if (numberOfNights <= 0)
                return BadRequest("Number of nights must be greater than 0.");

            try
            {
                var pricing = await hotelService.CalculateRoomPricingAsync(hotelId, roomType, numberOfNights);
                return Ok(pricing);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }





    }
}

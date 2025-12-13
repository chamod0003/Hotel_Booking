using Application_Layer;
using Application_Layer.DTO;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class CreateHotelService
    {
        [Fact]

        public async Task CreateHotelAsync_NullDto_ThrowsArgumentNullException()
        {
            var repomock = new Mock<IHotelRepository>();
            var service = new HotelService(repomock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateHotelAsync(null);
            });
        }

        [Fact]
        public async Task Createhotel_without_name_throws_exception()
        {
            // Arrange
            var repomock = new Mock<IHotelRepository>();
            var service = new HotelService(repomock.Object);
            var dto = new CreateHotelDto
            {
                HotelName = "", 
                Address = "123 Main St",
                Latitude = 40.7128m,
                Longitude = -74.0060m,
                DistrictId = 1,
                BriefDescription= "A nice hotel",
            };

            // Act & Assert
        
            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.CreateHotelAsync(dto);
            });

            Assert.Equal("Hotel Name is required.", exception.Message);
        }
    }
}

using Application_Layer;
using Application_Layer.DTO;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Moq;

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
                BriefDescription = "A nice hotel",
            };

            // Act & Assert

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.CreateHotelAsync(dto);
            });

            Assert.Equal("Hotel Name is required.", exception.Message);
        }

        [Fact]
        public async Task GetAllHotelsAsync_NoHotels_ReturnEmptyList()
        {
            var repomock = new Mock<IHotelRepository>();

            repomock
                .Setup(r=>r.GetAllAsync())
                .ReturnsAsync(new List<Hotel>());

            var service = new HotelService(repomock.Object);

            var result = await service.GetAllHotelsAsync();

            Assert.Empty(result);
            Assert.NotNull(result);
        }

        [Fact]

        public async Task CreateHotelAsync_ValidDto_CallsAddAsyncOnce()
        {
            //Arrange

            var remomock = new Mock<IHotelRepository>();

            remomock
                .Setup(r => r.AddAsync(It.IsAny<Hotel>()))
                .ReturnsAsync((Hotel h) =>
                     {
                        h.HotelId = 10;  
                        return h;
                  })                
                .Callback<Hotel>(h => h.HotelId = 10);


            var service = new HotelService(remomock.Object);

            var dto = new CreateHotelDto
            {
                HotelName = "Test Hotel",
                Latitude = 6.9m,
                Longitude = 79.8m,
            };

            //Act

            var result = await service.CreateHotelAsync(dto);

            //Assert

            remomock.Verify(r => r.AddAsync(It.IsAny<Hotel>()), Times.Once);
            Assert.Equal(10, result.HotelId);
        }

    }
}

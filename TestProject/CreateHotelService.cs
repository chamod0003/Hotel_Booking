using Application_Layer;
using Application_Layer.DTO;
using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject
{
    public class CreateHotelService
    {
        [Fact]

        public async Task CreateHotelAsync_NullDto_ThrowsArgumentNullException()
        {
            var repomock = new Mock<IHotelRepository>();
            var cacheMock = new Mock<IMemoryCache>();
            var loggerMock = new Mock<ILogger<HotelService>>();
            var service = new HotelService(repomock.Object, cacheMock.Object, loggerMock.Object);

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
            var cacheMock = new Mock<IMemoryCache>();
            var loggerMock = new Mock<ILogger<HotelService>>();
            var service = new HotelService(repomock.Object, cacheMock.Object, loggerMock.Object);
            var dto = new CreateHotelDto
            {
                HotelName = "",
                Address = "123 Main St",
                Latitude = 40.7128m,
                Longitude = -74.0060m,
                DistrictId = 1,
                BriefDescription = "A nice hotel",
            };

            // Act 

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await service.CreateHotelAsync(dto);
            });

            //Assert

            Assert.Equal("Hotel Name is required.", exception.Message);
        }

        [Fact]
        public async Task CreateHotelAsync_ValidDto_CallsAddAsyncOnce()
        {
            var repoMock = new Mock<IHotelRepository>();
            var cacheMock = new Mock<IMemoryCache>();
            var loggerMock = new Mock<ILogger<HotelService>>();
            var service = new HotelService(repoMock.Object, cacheMock.Object, loggerMock.Object);

            repoMock.Setup(r => r.AddAsync(It.IsAny<Hotel>()))
                    .ReturnsAsync((Hotel h) =>
                    {
                        h.HotelId = 10;
                        return h;
                    });

         

            var dto = new CreateHotelDto
            {
                HotelName = "Test Hotel"
            };

            var result = await service.CreateHotelAsync(dto);

            repoMock.Verify(r => r.AddAsync(It.IsAny<Hotel>()), Times.Once);
            Assert.Equal(10, result.HotelId);
        }


        [Fact]

        public async Task CreateHotelAsync_InvalidDto_DoesNotCallRepository()
        {
            var repomock = new Mock<IHotelRepository>();
            var cacheMock = new Mock<IMemoryCache>();
            var loggerMock = new Mock<ILogger<HotelService>>();
            var service = new HotelService(repomock.Object, cacheMock.Object, loggerMock.Object);

            await Assert.ThrowsAsync<Exception>(()=>
                service.CreateHotelAsync(new CreateHotelDto())
            );

            repomock.Verify(r => r.AddAsync(It.IsAny<Hotel>()), Times.Never);
        }




        [Fact]
        public async Task GetAllHotelsAsync_NoHotels_ReturnEmptyList()
        {
            var repomock = new Mock<IHotelRepository>();
            var cacheMock = new Mock<IMemoryCache>();
            var loggerMock = new Mock<ILogger<HotelService>>();
            var service = new HotelService(repomock.Object, cacheMock.Object, loggerMock.Object);

            repomock
                .Setup(r=>r.GetAllAsync())
                .ReturnsAsync(new List<Hotel>());

           

            var result = await service.GetAllHotelsAsync();

            Assert.Empty(result);
            Assert.NotNull(result);
        }

        

    }
}

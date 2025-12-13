using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Queries
{
    public record class GetHotelsByAmentyQuery(AmenityType Amenity) : IRequest<List<Hotel>>
    {
        public class GetHotelsByAmentyQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<GetHotelsByAmentyQuery, List<Hotel>>
        {
            public async Task<List<Hotel>> Handle(GetHotelsByAmentyQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.GetHotelsByAmenityAsync(request.Amenity);
                return hotels.ToList();
            }
        }
    }
}

using Domain_Layer.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Queries
{
    public record GetHotelByLocationQuery(decimal latitude, decimal longitude,decimal radius) : IRequest<List<Hotel>>
    {
        public class GetHotelByLocationHandler(Domain_Layer.Interface.IHotelRepository hotelRepository) : IRequestHandler<GetHotelByLocationQuery, List<Hotel>>
        {
            public async Task<List<Hotel>> Handle(GetHotelByLocationQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.GetHotelsByLocationAsync(request.latitude, request.longitude, request.radius);
                return hotels.ToList();
            }
        }
    }
}

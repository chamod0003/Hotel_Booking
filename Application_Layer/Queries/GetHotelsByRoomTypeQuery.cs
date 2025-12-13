using Domain_Layer.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Queries
{
    public record GetHotelsByRoomTypeQuery(RoomType RoomType) : IRequest<List<Hotel>>
    {
        public class GetHotelsByRoomTypeQueryHandler(Domain_Layer.Interface.IHotelRepository hotelRepository) : IRequestHandler<GetHotelsByRoomTypeQuery, List<Hotel>>
        {
            public async Task<List<Hotel>> Handle(GetHotelsByRoomTypeQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.GetHotelsByRoomTypeAsync(request.RoomType);
                return hotels.ToList();
            }
        }
    }
}

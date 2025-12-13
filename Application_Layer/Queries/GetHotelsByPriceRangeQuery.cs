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
    public record GetHotelsByPriceRangeQuery(decimal MinPrice, decimal MaxPrice) : IRequest<List<Hotel>>
    {
        public class GetHotelsByPriceRangeQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<GetHotelsByPriceRangeQuery, List<Domain_Layer.Models.Entity.Hotel>>
        {
            public async Task<List<Domain_Layer.Models.Entity.Hotel>> Handle(GetHotelsByPriceRangeQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.GetHotelsByPriceRangeAsync(request.MinPrice, request.MaxPrice);
                return hotels.ToList();
            }
        }
    }
}

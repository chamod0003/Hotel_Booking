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
    public record GetAllQuery : IRequest<List<Hotel>>
    {
        public class GetAllQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<GetAllQuery, List<Hotel>>
        {
            public async Task<List<Hotel>> Handle(GetAllQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.GetAllAsync();
                return hotels.ToList();
            }
        }
    }
}

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
    public record GetByIdQuery(int hotelId):IRequest<Hotel>
    {
        public class GetByEmailHandler(IHotelRepository hotelRepository) : IRequestHandler<GetByIdQuery, Hotel>
        {
            public async Task<Hotel> Handle(GetByIdQuery request, CancellationToken cancellationToken)
            {
                var hotel = await hotelRepository.GetByIdAsync(request.hotelId);
                return hotel;
            }
        }
    }
}

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
    public record SearchByNameQuery(string Name) : IRequest<List<Hotel>>
    {
        public class SearchByNameQueryHandler(IHotelRepository hotelRepository) : IRequestHandler<SearchByNameQuery, List<Hotel>>
        {
            public async Task<List<Hotel>> Handle(SearchByNameQuery request, CancellationToken cancellationToken)
            {
                var hotels = await hotelRepository.SearchByNameAsync(request.Name);
                return hotels.ToList();
            }
        }
    }
}

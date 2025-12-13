using Domain_Layer.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Commond
{
    public record DeleteHotelCommand(int HotelId) : IRequest<bool>;

    public class DeleteHotelHandler(IHotelRepository hotelRepository) : IRequestHandler<DeleteHotelCommand, bool>
    {
        public async Task<bool> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var result = await hotelRepository.DeleteAsync(request.HotelId);
            return result;
        }
    }
}

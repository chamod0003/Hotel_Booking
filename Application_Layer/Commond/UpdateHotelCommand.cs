using Domain_Layer.Interface;
using Domain_Layer.Models.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Commond
{
    public record UpdateHotelCommand(Hotel Hotel):IRequest<Hotel>;
    public class UpdateHotelHandler(IHotelRepository hotelRepository) : IRequestHandler<UpdateHotelCommand, Hotel>
    {
        public async Task<Hotel> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var updatedHotel = await hotelRepository.UpdateAsync(request.Hotel);
            return updatedHotel;
        }
    }
  
}

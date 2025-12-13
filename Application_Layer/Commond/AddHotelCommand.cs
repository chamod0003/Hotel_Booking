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

    public record AddHotelCommand(Hotel Hotel):IRequest<Hotel>;

    public class AddHotelHandler(IHotelRepository hotelRepository): IRequestHandler<AddHotelCommand, Hotel>
    {
        public async Task<Hotel> Handle(AddHotelCommand request, CancellationToken cancellationToken)
        {
            var addedHotel = await hotelRepository.AddAsync(request.Hotel);
            return addedHotel;
        }
    }
}

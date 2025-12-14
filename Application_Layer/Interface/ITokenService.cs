using Domain_Layer.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

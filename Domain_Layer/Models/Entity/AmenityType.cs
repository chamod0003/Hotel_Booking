using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Models.Entity
{
    public class AmenityType
    {
        [Key]
        public int AmenityTypeId { get; set; }
        public string Name { get; set; }

        public  ICollection<Hotel> Hotels { get; set; }
    }
}

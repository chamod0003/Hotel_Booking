using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DTO
{
    public class PaymentDTO
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string CVV { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
    }

    public class PaymentResponseDTO
    {
        public string Reason { get; set; }
    }
}

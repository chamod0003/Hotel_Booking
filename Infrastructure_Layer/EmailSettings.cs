using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer
{
    public class EmailSettings
    {
        
            public string SmtpHost { get; set; } = "smtp.gmail.com";
            public int SmtpPort { get; set; } = 587;
            public string SmtpUser { get; set; }
            public string SmtpPassword { get; set; }
            public string FromEmail { get; set; }
            public string FromName { get; set; } = "Hotel Service";
            public bool EnableSsl { get; set; } = true;
    
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class AppSettings
    {
        public string SMTPADDRESS { get; set; }
        public string PORT { get; set; }
        public string DHLId { get; set; }
        public string DHLPassword { get; set; }
        public string ADMINEMAIL { get; set; }
        public string SMTPUSERNAME { get; set; }
        public string SMTPPASSWORD { get; set; }
        public string SENDGRID_API_KEY { get; set; }
        public string FedExUserId { get; set; }        public string FedExPassword { get; set; }        public string FedExAccountNumber { get; set; }        public string FedExMeterNumber { get; set; }
    }
}

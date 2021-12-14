using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class User : Record
    {
        public string UserName{ get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int SmsCredit { get; set; }
        public decimal CostPerSms { get; set; }
        public bool AdminApproval { get; set; }
    }
}

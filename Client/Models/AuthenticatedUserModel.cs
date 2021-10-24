using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class AuthenticatedUserModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}

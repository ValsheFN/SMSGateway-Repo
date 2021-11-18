using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class SubAccount : UserRecord
    {
        [Required]
        public string UserId { get; set; }
    }
}

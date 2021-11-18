using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class ApplicationUserModel :IdentityUser
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPerSms { get; set; } = 500;
        public int SmsCredit { get; set; } = 0;
    }
}

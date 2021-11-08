using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class UserModel : IdentityUser
    {
        [StringLength(60, MinimumLength = 6)]
        public string Password { get; set; }
        [StringLength(60, MinimumLength = 6)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPerSms { get; set; } = 500;
        public int SmsCredit { get; set; } = 0;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
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

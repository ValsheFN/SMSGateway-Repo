using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Shared
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(60)]
        public string Email { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Password { get; set; }
    }
}

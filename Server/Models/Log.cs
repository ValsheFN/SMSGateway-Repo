using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class Log : Record
    {
        [Required]
        public string From { get; set; }
        [Required]
        public string SendTo { get; set; }
        [Required]
        public string Messages { get; set; }
        [Required]
        public DateTime TimeSent { get; set; }
    }
}

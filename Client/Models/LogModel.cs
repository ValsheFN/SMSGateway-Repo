using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class LogModel
    {
        public string From { get; set; }
        public string SendTo { get; set; }
        public string Messages { get; set; }
        public DateTime TimeSent { get; set; }
    }
}

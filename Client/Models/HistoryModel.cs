using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class HistoryModel
    {
        public string ReferenceId { get; set; }
        public string Recipients { get; set; }
        public string Messages { get; set; }
        public DateTime TimeSent { get; set; }
        public string Status { get; set; }
    }
}

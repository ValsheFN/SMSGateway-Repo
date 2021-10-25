using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class TopUpModel
    {
        public string ReferenceId { get; set; }
        public decimal TopUpValue { get; set; }
        public string Requester { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime GrantDate { get; set; }
        public string GrantedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class SmsModel
    {
        public string From { get; set; }
        public string SendTo { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        public string ContactGroup { get; set; }
        public string SmsTemplateList { get; set; }
    }
}

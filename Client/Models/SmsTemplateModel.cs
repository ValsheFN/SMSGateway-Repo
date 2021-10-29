using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class SmsTemplateModel
    {
        public string ReferenceId { get; set; }
        public string SmsTemplateName { set; get; }
        public string Content { set; get; }
    }
}

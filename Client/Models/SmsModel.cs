using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class SmsModel<T>
    {
        public string Messages { get; set; }
        public List<T> SendTo { get; set; }
        public List<T> ContactGroup { get; set; }
        public List<T> SmsTemplateList { get; set; }
    }
}

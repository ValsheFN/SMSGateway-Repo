using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpVitamins;

namespace SMSGateway.Server.Models
{
    public class SmsTemplate : UserRecord
    {
        public SmsTemplate()
        {
            ShortGuid guid = Guid.NewGuid().ToString();
            ReferenceId = guid;
        }

        public string ReferenceId { get; set; }
        [Required]
        public string SmsTemplateName { set; get; }
        [Required]
        public string Content { set; get; }
        //Foreign Keys
    }
}

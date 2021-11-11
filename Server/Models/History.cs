using CSharpVitamins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class History : UserRecord
    {
        public History()
        {
            ShortGuid guid = Guid.NewGuid().ToString();
            ReferenceId = guid;
        }

        public string ReferenceId { get; set; }
        [Required]
        public string Recipients { get; set; }
        [Required]
        public string Messages { get; set; }
        [Required]
        public DateTime TimeSent { get; set; }
        [Required]
        public string Status { get; set; }
    }
}

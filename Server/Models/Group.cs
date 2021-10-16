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
    public class Group : UserRecord
    {
        public Group()
        {
            ShortGuid guid = Guid.NewGuid().ToString();
            ReferenceId = guid;
        }

        public string ReferenceId { get; set; }
        [Required]
        public string GroupName { get; set; }
    }
}

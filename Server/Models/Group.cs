using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class Group : UserRecord
    {
        public Group()
        {
            ReferenceId = Guid.NewGuid().ToString();
        }

        public string ReferenceId { get; set; }
        [Required]
        public string GroupName { get; set; }
    }
}

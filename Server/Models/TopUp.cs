using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class TopUp : UserRecord
    {
        public TopUp()
        {
            ReferenceId = Guid.NewGuid().ToString();
        }

        public string ReferenceId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TopUpValue { get; set; }
        public string Requester { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime GrantDate { get; set; }
        public string GrantedBy { get; set; }
    }
}

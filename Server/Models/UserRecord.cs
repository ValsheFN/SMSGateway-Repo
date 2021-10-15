using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public abstract class UserRecord : Record
    {
        public UserRecord()
        {
            CreationDate = DateTime.Now;
            UpdateDate = DateTime.Now;

        }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        //Foreign Keys
        public virtual ApplicationUser CreatedByUser { get; set; }
        [ForeignKey(nameof(CreatedByUser))]
        public string CreatedByUserId { get; set; }
        public virtual ApplicationUser UpdatedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUser))]
        public string UpdatedByUserId { get; set; }

    }
}

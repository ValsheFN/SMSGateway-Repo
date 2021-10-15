using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SMSGateway.Server.Models;

namespace SMSGateway.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedContact = new List<Contact>();
            UpdatedContact = new List<Contact>();
            CreatedContactGroup = new List<ContactGroup>();
            UpdatedContactGroup = new List<ContactGroup>();
            CreatedGroup = new List<Group>();
            UpdatedGroup = new List<Group>();
            CreatedSmsTemplate = new List<SmsTemplate>();
            UpdatedSmsTemplate = new List<SmsTemplate>();
            CreatedTopUp = new List<TopUp>();
            UpdatedTopUp = new List<TopUp>();
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CreditValue { get; set; } = 0;

        //Relationships
        public virtual List<Contact> CreatedContact { get; set; }
        public virtual List<Contact> UpdatedContact { get; set; }
        public virtual List<ContactGroup> CreatedContactGroup { get; set; }
        public virtual List<ContactGroup> UpdatedContactGroup { get; set; }
        public virtual List<Group> CreatedGroup { get; set; }
        public virtual List<Group> UpdatedGroup { get; set; }
        public virtual List<SmsTemplate> CreatedSmsTemplate { get; set; }
        public virtual List<SmsTemplate> UpdatedSmsTemplate { get; set; }
        public virtual List<TopUp> CreatedTopUp { get; set; }
        public virtual List<TopUp> UpdatedTopUp { get; set; }

    }
}

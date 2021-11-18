using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
            CreatedHistory = new List<History>();
            UpdatedHistory = new List<History>();
            CreatedSubAccount = new List<SubAccount>();
            UpdatedSubAccount = new List<SubAccount>();
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPerSms { get; set; } = 500;
        public int SmsCredit { get; set; } = 0;

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
        public virtual List<History> CreatedHistory { get; set; }
        public virtual List<History> UpdatedHistory { get; set; }
        public virtual List<SubAccount> CreatedSubAccount { get; set; }
        public virtual List<SubAccount> UpdatedSubAccount { get; set; }

    }
}

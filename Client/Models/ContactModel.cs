using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class ContactModel
    {
        public string Id { get; set; }
        public string ReferenceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public int ContactGroupId { get; set; }
    }
}

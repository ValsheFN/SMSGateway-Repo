using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class GroupModel
    {
        public string ReferenceId { get; set; }
        public string GroupName { get; set; }
    }
}

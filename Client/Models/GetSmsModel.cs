using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Models
{
    public class GetSmsModel
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public List<Param> Params { get; set; }
    }

    public class Param
    {
        public string Access_Token { get; set; }
        public string Folder { get; set; }
        public string IdFrom { get; set; }
        public string IdTo { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class ReadSms
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public Params Params { get; set; }
    }

    public class Params
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("folder")]
        public string Folder { get; set; }
        [JsonProperty("idfrom")]
        public string IdFrom { get; set; }
        [JsonProperty("idto")]
        public string IdTo { get; set; }
    }
}

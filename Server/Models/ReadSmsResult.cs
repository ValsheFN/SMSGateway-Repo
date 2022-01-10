using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Models
{
    public class ReadSmsResult
    {
            public int id { get; set; }
            public Result[] result { get; set; }
    }

    public class Result
    {
        public string UpdatedInDB { get; set; }
        public string InsertIntoDB { get; set; }
        public string SendingDateTime { get; set; }
        public object DeliveryDateTime { get; set; }
        public string Text { get; set; }
        public string DestinationNumber { get; set; }
        public string Coding { get; set; }
        public string UDH { get; set; }
        public string SMSCNumber { get; set; }
        public string Class { get; set; }
        public string TextDecoded { get; set; }
        public string ID { get; set; }
        public string SenderID { get; set; }
        public string SequencePosition { get; set; }
        public string Status { get; set; }
        public string StatusError { get; set; }
        public string TPMR { get; set; }
        public string RelativeValidity { get; set; }
        public string CreatorID { get; set; }
        public string id_folder { get; set; }
        public string StatusCode { get; set; }
        public object MMS_ID { get; set; }
        public object MMSHeaders { get; set; }
        public object MMSReports { get; set; }
    }



}

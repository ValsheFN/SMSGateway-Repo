using Newtonsoft.Json;
using SMSGateway.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Json;
using SMSGateway.Shared;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using EFCore.BulkExtensions;

namespace SMSGateway.Server.Services
{
    public interface ISchedulerService
    {
        Task<UserManagerResponse> UpdateSmsStatus();
    }

    public class SchedulerService : ISchedulerService
    {
        private IConfiguration Configuration { get; }
        private readonly ApplicationDBContext _db;

        public SchedulerService(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            Configuration = configuration;
        }

        public async Task<UserManagerResponse> UpdateSmsStatus()
        {
            HttpClient httpClient = new HttpClient();
            var url = Configuration["AppUrl"] + "/api/history?status=Pending&hasMessageId=true";
            //Get sms history with status pending
            List<History> ListOfHistory = await httpClient.GetFromJsonAsync<List<History>>(url);

            if (ListOfHistory.Count() == 0)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "No data to be updated"
                };
            }

            try
            {
                //Check the lowest id and highest id
                var min = ListOfHistory.Min(x => x.MessageId);
                var max = ListOfHistory.Max(x => x.MessageId);

                //Call smseagle and get from folder sent
                ReadSms payload = new ReadSms
                {
                    Id = 1,
                    Method = "sms.read_sms",
                    Params = new Params
                    {
                        AccessToken = "aFygNaan7p4C1ofkY2FkIdtpOZvIb2ky",
                        Folder = "sentitems",
                        IdFrom = min,
                        IdTo = max
                    }
                };

                var updatedPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
                var buffer = System.Text.Encoding.UTF8.GetBytes(updatedPayload);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var readSmsUrl = "https://hwd.dyndns.org/jsonrpc/sms";

                //var response = await httpClient.PostAsJsonAsync(readSmsUrl, newPayload);
                //var responseBody = await response.Content.ReadAsStringAsync();
                //var results = JsonConvert.DeserializeObject<List<ReadSmsResult>>(responseBody);

                var response = await httpClient.PostAsync(readSmsUrl, byteContent);
                var responseBody = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<ReadSmsResult>(responseBody);
                List<History> historyList = new List<History>();

                //Compare between sms history and smseagle data and update the status
                foreach (var data in ListOfHistory)
                {
                    var status = (results.result.Where(x => x.ID.ToString() == data.MessageId).FirstOrDefault()).Status;

                    if (status.Contains("OK"))
                    {
                        status = "Sent";
                    }
                    else
                    {
                        status = "Error";
                    }

                    historyList.Add(new History
                    {
                     
                        Id = data.Id,
                        ReferenceId = data.ReferenceId,
                        MessageId = data.MessageId,
                        Recipients = data.Recipients,
                        Messages = data.Messages,
                        TimeSent = data.TimeSent,
                        Status = status
                    });
                }
                _db.BulkUpdate(historyList);

                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Sms status is updated"
                };
            }
            catch (Exception e)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = e.InnerException.ToString()
                };
            }
        }
    }
}

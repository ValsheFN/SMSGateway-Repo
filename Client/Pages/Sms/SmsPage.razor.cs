using Microsoft.AspNetCore.Components;
using SMSGateway.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SMSGateway.Shared;

namespace SMSGateway.Client.Pages.Sms
{
    public partial class SmsPage
    {

        public async Task SendSms(SmsModel model)
        {
            //Check error
            if (string.IsNullOrWhiteSpace(model.SendTo) && string.IsNullOrWhiteSpace(contactValue) && string.IsNullOrWhiteSpace(groupValue))
            {
                _snackbar.Add("Recipient is required!", Severity.Error);
            }

            else if (string.IsNullOrWhiteSpace(model.Content) && string.IsNullOrWhiteSpace(templateValue))
            {
                _snackbar.Add("Message is required!", Severity.Error);
            }
            //End of check error

            else
            {

                //Get SMS content from either template or message
                var content = "";
                if (string.IsNullOrWhiteSpace(templateValue))
                {
                    content = model.Content;
                }
                else
                {
                    var templateList = await _httpClient.GetFromJsonAsync<List<SmsTemplateModel>>("/api/smsTemplate/GetSmsTemplate");
                    var selectedTemplate = templateList.Where(x => x.ReferenceId == templateValue).SingleOrDefault();
                    content = selectedTemplate.Content;
                }

                //Get recipients from either phone number or contact
                var phoneNumber = "";
                var userId = _localStorage.GetItemAsString("user_id");
                var token = _localStorage.GetItemAsString("access_token");

                if (string.IsNullOrWhiteSpace(model.SendTo))
                {
                    phoneNumber = contactValue;

                    List<string> collection = new List<string>(groupValue.Split(new string[] { "," }, StringSplitOptions.None));

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var contactGroupList = await _httpClient.GetFromJsonAsync<List<ContactGroupModel>>("/api/contactgroup/GetContactGroup?createdByUserId=" + userId);
                    var list = contactGroupList.Where(x => x.GroupName.Any(y => collection.Contains(y.ToString())));

                    int iteration = 0;
                    foreach (var item in list)
                    {
                        iteration++;
                        if (iteration == list.Count())
                        {
                            phoneNumber += item.PhoneNumber;
                        }
                        else
                        {
                            phoneNumber += item.PhoneNumber + ",";
                        }
                    }
                }
                else
                {
                    phoneNumber = model.SendTo;
                }

                var basePath = "http://hwd.dyndns.org";
                var smsToken = "aFygNaan7p4C1ofkY2FkIdtpOZvIb2ky";
                var message = content;
                var sendTo = phoneNumber;

                /*var basePath = _configuration["SmsEagle:BasePath"];
                var token = _configuration["SmsEagle:Token"];*/

                /*var response = await _httpClient.GetAsync($"{basePath}/http_api/sendsms?access_token={smsToken}&to={sendTo}&message={message}&unicode=1");

                if (response.IsSuccessStatusCode == true)
                {
                    _snackbar.Add("SMS Sent", Severity.Success);
                    smsConfig.SendTo = "";
                    smsConfig.Content = "";
                    StateHasChanged();
                }
                else
                {
                    _snackbar.Add("SMS failed to be sent", Severity.Error);
                    smsConfig.SendTo = "";
                    smsConfig.Content = "";
                    StateHasChanged();
                }*/

                //Send to log

                Clear();

                var userData = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user?userId=" + userId);
                
                var timeSent = DateTime.Now;

                var logData = new LogModel
                {
                    From = userData.FirstOrDefault().UserName,
                    SendTo = phoneNumber,
                    Messages = message,
                    TimeSent = timeSent
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await _httpClient.PostAsJsonAsync<LogModel>("/api/log/CreateLog", logData);

                //Send to history

                var historyData = new HistoryModel
                {
                    Recipients = phoneNumber,
                    Messages = message,
                    Status = "Pending",
                    TimeSent = timeSent
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await _httpClient.PostAsJsonAsync<HistoryModel>("/api/history", historyData);
            }
        }

        private void Clear()
        {
            RemovePhoneNumber();
            RemoveTemplate();
            RemoveMessage();
            RemoveContact();
            StateHasChanged();
        }
    }
}

using SMSGateway.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MudBlazor;
using System.Net.Http.Headers;
using System.Net.Http;
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
                var userInfo = _localStorage.GetItem<List<UserModel>>("user_info");
                var token = _localStorage.GetItemAsString("access_token");

                //Get user settings
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userDetail = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user?userId=" + userId);

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

                //Count whether the credit is enough
                //TO DO : GET FROM DB INSTEAD OF HARDCODE 160CHAR = 1 SMS
                decimal totalRecipients = (phoneNumber.Split(',')).Length;
                decimal smsCount = content.Length / 160;
                decimal remainder = content.Length % 160;

                if(remainder > 0)
                {
                    smsCount++;
                }
                
                decimal smsCost = userDetail.FirstOrDefault().CostPerSms * smsCount * totalRecipients;
                decimal smsCredit = userDetail.FirstOrDefault().SmsCredit;

                if(smsCredit < smsCost)
                {
                    _snackbar.Add($"You don't have enough credit to send SMS. This SMS will cost you {smsCost}", Severity.Error);
                }

                //Apply specific chosen settings
                var time = "";
                if(SelectedTime == "At date and time")
                {
                    var smsDate = (sendDate.ToString().Split(" "))[0].Replace("/", "");
                    var formattedSmsDate = smsDate[4..8] + smsDate[0..2] + smsDate[2..4];
                    var smsTime = sendTime.ToString().Replace(":", "");
                    time = $"&date={formattedSmsDate}{smsTime}";
                }
                else if(SelectedTime == "After a delay")
                {
                    var smsTime = sendAfter.ToString();
                    time = $"&send_after={smsTime}";
                }
                else if(SelectedTime == "Between hours")
                {
                    var smsStartTime = startTime.ToString();
                    var smsEndTime = endTime.ToString();
                    time = $"&send_after={smsStartTime}&send_before={smsEndTime}";
                }

                var messageType = "";
                if(SelectedType == "Flash SMS")
                {
                    messageType = $"&flash=1";
                }
                else if(SelectedType == "WAP Push Link")
                {
                    messageType = $"";
                }

                var basePath = "https://hwd.dyndns.org";
                var path = "/http_api/send_sms";
                var smsToken = "aFygNaan7p4C1ofkY2FkIdtpOZvIb2ky";
                var message = content;
                var sendTo = phoneNumber;
                
                var fullPath = $"{basePath}{path}?access_token={smsToken}&to={sendTo}&message={message}&unicode=1{time}{messageType}";

                /*var basePath = _configuration["SmsEagle:BasePath"];
                var token = _configuration["SmsEagle:Token"];*/

                try
                {
                    HttpClient httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(fullPath);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    var result = responseBody.Contains("OK") ? "Pending" : "Error";

                    if(result == "ERROR")
                    {
                        var errorMessage = responseBody;
                        _snackbar.Add(errorMessage, Severity.Error);
                        return;
                    }

                    var resultId = ((responseBody.Split("ID=")[1]).Split("\n"))[0];

                    if (response.IsSuccessStatusCode == true)
                    {
                        //Reduce credit
                        if (smsCredit > 0)
                        {

                        }

                        _snackbar.Add("SMS Sent", Severity.Success);
                        StateHasChanged();

                        //var userData = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user?userId=" + userId);

                        var timeSent = DateTime.Now;

                        var logData = new LogModel
                        {
                            MessageId = resultId,
                            From = userInfo.FirstOrDefault().UserName,
                            SendTo = phoneNumber,
                            Messages = message,
                            TimeSent = timeSent
                        };

                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var logResponse = await _httpClient.PostAsJsonAsync<LogModel>("/api/log/CreateLog", logData);
                        var logResponseBody = await logResponse.Content.ReadAsStringAsync();
                        var logResults = JsonConvert.DeserializeObject<OperationResponse<LogModel>>(logResponseBody);

                        //Send to history

                        var historyData = new HistoryModel
                        {
                            MessageId = resultId,
                            Recipients = phoneNumber,
                            Messages = message,
                            Status = result,
                            TimeSent = timeSent
                        };

                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var historyResponse = await _httpClient.PostAsJsonAsync<HistoryModel>("/api/history", historyData);
                        var historyResponseBody = await historyResponse.Content.ReadAsStringAsync();
                        var historyResults = JsonConvert.DeserializeObject<OperationResponse<HistoryModel>>(historyResponseBody);
                        Clear();
                    }
                    else
                    {
                        _snackbar.Add("SMS failed to be sent", Severity.Error);
                        StateHasChanged();
                    }
                    
                }
                catch(Exception e)
                {
                    _snackbar.Add(e.Message, Severity.Error);
                    StateHasChanged();
                }
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

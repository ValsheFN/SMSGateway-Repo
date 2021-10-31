using Microsoft.AspNetCore.Components;
using SMSGateway.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.Extensions.Configuration;

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
                var basePath = "http://hwd.dyndns.org/";
                var token = "aFygNaan7p4C1ofkY2FkIdtpOZvIb2ky";
                var message = model.Content;
                var sendTo = model.SendTo;

                if (!string.IsNullOrWhiteSpace(contactValue))
                {
                  sendTo += $",{contactValue}";
                }

                if (!string.IsNullOrWhiteSpace(groupValue))
                {
                    var groups = groupValue.Split(',').ToList<string>();
                    foreach(var group in groups)
                    {
                        var phoneList = await _httpClient.GetFromJsonAsync<List<ContactModel>>("/api/contactGroup/GetContact");
                    }
                }
                
                /*var basePath = _configuration["SmsEagle:BasePath"];
                var token = _configuration["SmsEagle:Token"];*/
                
                var response = await _httpClient.GetAsync($"{basePath}/http_api/sendsms?access_token={token}&to={sendTo}&message={message}&unicode=1");

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
                }
            }

        }

        private async void SetContent(ChangeEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                var referenceId = e.Value.ToString();
                var templateContent = await _httpClient.GetFromJsonAsync<List<SmsTemplateModel>>("/api/smsTemplate/GetSmsTemplate?referenceId" + referenceId);
                smsConfig.Content = templateContent.Select(x => x.Content).ToString();
                StateHasChanged();
            }

        }

        private void Clear()
        {
            smsConfig.Subject = "";
            smsConfig.Content = "";
            groupValue = "";
            contactValue = "";
            StateHasChanged();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SMSGateway.Client.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using SMSGateway.Shared;
using System.Net.Http.Headers;

namespace SMSGateway.Client.Pages.Dialog
{
    public partial class AddTopUpDialog
    {
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async void CreateTopUp(decimal topUpValue)
        {
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");

            if (topUpValue <= 0)
            {
                Snackbar.Add("Top up value cannot be equal to or below 0", Severity.Success);
            }
            else
            {
                var authState = _authenticationStateProvider.GetAuthenticationStateAsync();
                //var email = authState.User.FindFirst("sub")?.Value;

                var topUp = new TopUpModel
                {
                    TopUpValue = topUpValue,
                    Requester = ""
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("/api/topUp/CreateTopUp/", topUp);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OperationResponse<TopUpModel>>(responseBody);

                if (result.IsSuccess)
                {
                    MudDialog.Close(DialogResult.Ok(""));
                    Snackbar.Add(result.Message, Severity.Success);
                }
                else
                {
                    MudDialog.Close(DialogResult.Ok(""));
                    Snackbar.Add(result.Message, Severity.Error);
                }
            }
        }
    }
}

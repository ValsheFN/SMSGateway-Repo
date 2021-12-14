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
using SMSGateway.Client.Pages.User;
using System.Collections.Generic;

namespace SMSGateway.Client.Pages.Dialog
{
    public partial class ApproveUserDialog
    {
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        protected async override void OnInitialized()
        {
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfUsers = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user");
        }

        private async void ApproveUser(UserModel model)
        {
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync<UserModel>("/api/user/approve", model);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse<RoleModel>>(responseBody);

            if (result.IsSuccess)
            {
                MudDialog.Close(DialogResult.Ok(""));
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }

        private async void RevokeUser(UserModel model)
        {
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync<UserModel>("/api/user/revoke", model);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse<RoleModel>>(responseBody);

            if (result.IsSuccess)
            {
                MudDialog.Close(DialogResult.Ok(""));
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }
    }
}

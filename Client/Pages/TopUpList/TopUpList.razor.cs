using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace SMSGateway.Client.Pages.TopUpList
{
    public partial class TopUpList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddTopUpDialog>("Add Top Up");
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfTopUps = await _httpClient.GetFromJsonAsync<List<TopUpModel>>("/api/topup/GetTopup");
            StateHasChanged();
        }

        private async Task Approve(TopUpModel topUp)
        {
            var parameters = new DialogParameters { ["TopUp"] = topUp };

            var dialog = DialogService.Show<ApproveTopUpDialog>("Approve Top Up", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfTopUps = await _httpClient.GetFromJsonAsync<List<TopUpModel>>("/api/topup/GetTopup");
            StateHasChanged();
        }

        private async Task Reject(TopUpModel topUp)
        {
            var parameters = new DialogParameters { ["TopUp"] = topUp };

            var dialog = DialogService.Show<RejectTopUpDialog>("Reject Top Up", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfTopUps = await _httpClient.GetFromJsonAsync<List<TopUpModel>>("/api/topup/GetTopup");
            StateHasChanged();
        }
    }
}

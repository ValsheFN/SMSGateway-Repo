using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace SMSGateway.Client.Pages.History
{
    public partial class History
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<HistoryModel> ListOfHistory = new List<HistoryModel>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated != true)
            {
                _navigation.NavigateTo("/login");
            }
            else
            {
                var token = _localStorage.GetItemAsString("access_token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                ListOfHistory = await _httpClient.GetFromJsonAsync<List<HistoryModel>>("/api/history");
            }
        }
        private async void ShowDetails(HistoryModel history)
{
            var parameters = new DialogParameters { ["History"] = history };

            var dialog = DialogService.Show<ShowHistoryDialog>("Show History Details", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfHistory = await _httpClient.GetFromJsonAsync<List<HistoryModel>>("/api/history");
            StateHasChanged();
        }
    }
}

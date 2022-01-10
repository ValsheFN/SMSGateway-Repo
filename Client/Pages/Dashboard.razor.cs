using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using SMSGateway.Client.Models;
using System.Net.Http.Headers;

namespace SMSGateway.Client.Pages
{
    public partial class Dashboard
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<UserModel> UserList = new List<UserModel>();
        private IEnumerable<HistoryModel> HistoryList = new List<HistoryModel>();

        private int smsCredit = 0;
        private string totalSmsSent = "";
        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated != true)
            {
                _navigation.NavigateTo("/login");
            }
            else
            {
                var userId = _localStorage.GetItemAsString("userId");
                var token = _localStorage.GetItemAsString("access_token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                UserList = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user?userId=" + userId);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HistoryList = await _httpClient.GetFromJsonAsync<List<HistoryModel>>("/api/history");

                smsCredit = UserList.FirstOrDefault().SmsCredit;
                totalSmsSent = "";

            }

        }
    }
}

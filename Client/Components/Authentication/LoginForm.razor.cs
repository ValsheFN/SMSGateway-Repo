using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SMSGateway.Client.Components
{
    public partial class LoginForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public ILocalStorageService Storage { get; set; }

        private LoginViewModel _model = new LoginViewModel();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task LoginUserAsync()
        {
            _isBusy = true;
            _errorMessage = string.Empty;

            var response = await HttpClient.PostAsJsonAsync("/api/auth/login", _model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                // Store it in local storage 
                await Storage.SetItemAsStringAsync("access_token", result.Message);
                await Storage.SetItemAsync<DateTime>("expiry_date", result.ExpiryDate);

                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Message);

                Navigation.NavigateTo("/");
            }
            else
            {
                var errorResult = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                _errorMessage = errorResult.Message;
            }

            _isBusy = false;
        }

    }
}

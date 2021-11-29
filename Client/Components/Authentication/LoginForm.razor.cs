using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SMSGateway.Shared;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SMSGateway.Client.Models;
using System.Collections.Generic;
using MudBlazor;

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

        private readonly UserManager<UserModel> _userManager;

        private LoginViewModel _model = new LoginViewModel();
        //private UserModel user = new UserMode();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task LoginUserAsync()
        {
            _isBusy = true;
            _errorMessage = string.Empty;

            var token = _localStorage.GetItemAsString("access_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                Snackbar.Add("You are currently logged in", Severity.Error);
                _navigation.NavigateTo("/");
            }
            else
            {
                var response = await HttpClient.PostAsJsonAsync("/api/auth/login", _model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();
                    //Get user information

                    var user = await HttpClient.GetFromJsonAsync<List<UserModel>>("/api/user?userId=" + result.UserId);
                    // Store it in local storage 
                    await Storage.SetItemAsStringAsync("user_id", result.UserId);
                    await Storage.SetItemAsync("user_info", user);
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
}

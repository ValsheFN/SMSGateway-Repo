using Microsoft.AspNetCore.Components;
using SMSGateway.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMSGateway.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace SMSGateway.Client.Components
{
    public partial class RegisterForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private RegisterViewModel _model = new();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task RegisterUserAsync()
        {
            _isBusy = true;
            _errorMessage = string.Empty;

            var response = await HttpClient.PostAsJsonAsync("/api/auth/register", _model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserManagerResponse>();

                Navigation.NavigateTo("/login");
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
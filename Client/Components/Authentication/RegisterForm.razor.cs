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
using MudBlazor;
using Newtonsoft.Json;
using SMSGateway.Client.Models;

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
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse<GroupModel>>(responseBody);

            if (result.IsSuccess)
            {
                NavigationManager.NavigateTo("/accountCreated");
            }
            else
            {
                _errorMessage = result.Message.ToString();
            }

            _isBusy = false;
        }

    }
}
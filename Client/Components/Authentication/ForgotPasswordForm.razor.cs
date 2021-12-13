using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MudBlazor;
using Newtonsoft.Json;
using SMSGateway.Client.Models;

namespace SMSGateway.Client.Components
{
    public partial class ForgotPasswordForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public ILocalStorageService Storage { get; set; }

        private ForgetPasswordViewModel _model = new ForgetPasswordViewModel();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;

        private async Task ForgotPasswordAsync(ForgetPasswordViewModel _model)
        {
            var response = await HttpClient.PostAsJsonAsync("api/auth/forgetpassword", _model);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse<GroupModel>>(responseBody);

            if (result.IsSuccess)
            {
                NavigationManager.NavigateTo("/passwordResetSent");
            }
            else
            {
                _errorMessage = result.Message.ToString();
            }
            
        }
    }
}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SMSGateway.Client.Models;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SMSGateway.Client.Components
{
    public partial class ChangePasswordForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public ILocalStorageService Storage { get; set; }

        private ResetPasswordViewModel _model = new ResetPasswordViewModel();
        private bool _isBusy = false;
        private string _errorMessage = string.Empty;
        private async Task ChangePasswordAsync(ResetPasswordViewModel _model)
        {
            _model.Email = Email;
            _model.Token = Token;

            var response = await HttpClient.PostAsJsonAsync("api/auth/resetpassword", _model);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse<ResetPasswordViewModel>>(responseBody);

            if (result.IsSuccess)
            {
                NavigationManager.NavigateTo("/passwordResetted");
            }
            else
            {
                _errorMessage = result.Message.ToString();
            }
        }
    }
}

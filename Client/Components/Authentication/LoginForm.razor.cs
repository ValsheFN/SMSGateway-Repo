using Microsoft.AspNetCore.Components;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SMSGateway.Client.Components
{
    public partial class LoginForm : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }
        private LoginViewModel _model = new LoginViewModel();
        private async Task LoginUserAsync()
        {
            throw new NotImplementedException();
        }

    }
}

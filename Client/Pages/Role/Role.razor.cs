using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.Role
{
    public partial class Role
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<RoleModel> ListOfRole = new List<RoleModel>();

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
                ListOfRole = await _httpClient.GetFromJsonAsync<List<RoleModel>>("/api/role");
            }
    }

        private async void Create()
        {
            var dialog = DialogService.Show<AddRoleDialog>("Add Role");
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfRole = await _httpClient.GetFromJsonAsync<List<RoleModel>>("/api/role");
            StateHasChanged();
        }

        private async Task Delete(RoleModel role)
        {
            var parameters = new DialogParameters { ["Role"] = role };

            var dialog = DialogService.Show<DeleteRoleDialog>("Delete Role", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfRole = await _httpClient.GetFromJsonAsync<List<RoleModel>>("/api/role");
            StateHasChanged();
        }

        private async Task Edit(RoleModel role)
        {
            var parameters = new DialogParameters { ["Role"] = role };

            var dialog = DialogService.Show<EditRoleDialog>("Edit Role", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfRole = await _httpClient.GetFromJsonAsync<List<RoleModel>>("/api/role");
            StateHasChanged();
        }
    }
}

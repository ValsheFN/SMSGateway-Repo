using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using SMSGateway.Client.Pages.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using SMSGateway.Client.Models;
using System.Net.Http.Headers;

namespace SMSGateway.Client.Pages.User
{
    public partial class User
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<UserModel> UserList = new List<UserModel>();

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
                UserList = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user");
            }
        }

        private async void Create()
        {
            var dialog = DialogService.Show<AddUserDialog>("Add User");
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            UserList = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user");
            StateHasChanged();
        }

        private async Task Delete(UserModel user)
        {
            var parameters = new DialogParameters { ["User"] = user };

            var dialog = DialogService.Show<DeleteUserDialog>("Delete Role", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            UserList = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user");
            StateHasChanged();
        }

        private async Task Edit(UserModel user)
        {
            var parameters = new DialogParameters { ["User"] = user };

            var dialog = DialogService.Show<EditUserDialog>("Edit User", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            UserList = await _httpClient.GetFromJsonAsync<List<UserModel>>("/api/user");
            StateHasChanged();
        }
    }
}

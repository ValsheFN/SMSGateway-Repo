using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.GroupList
{
    public partial class GroupList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<GroupModel> ListOfGroups = new List<GroupModel>();

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated != true)
            {
                _navigation.NavigateTo("/login");
            }
            else
            {
                var userId = _localStorage.GetItemAsString("user_id");
                var token = _localStorage.GetItemAsString("access_token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                ListOfGroups = await _httpClient.GetFromJsonAsync<List<GroupModel>>("/api/group/GetGroup?createdByUserId=" + userId);
            }
        }
        private async void Create()
        {
            var dialog = DialogService.Show<AddGroupDialog>("Add Group");
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfGroups = await _httpClient.GetFromJsonAsync<List<GroupModel>>("/api/group/GetGroup?createdByUserId=" + userId);
            StateHasChanged();
        }

        private async Task Delete(GroupModel group)
        {
            var parameters = new DialogParameters { ["Group"] = group };

            var dialog = DialogService.Show<DeleteGroupDialog>("Delete Group", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfGroups = await _httpClient.GetFromJsonAsync<List<GroupModel>>("/api/group/GetGroup?createdByUserId" + userId);
            StateHasChanged();
        }

        private async Task Edit(GroupModel group)
        {
            var parameters = new DialogParameters { ["Group"] = group };

            var dialog = DialogService.Show<EditGroupDialog>("Edit Group", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfGroups = await _httpClient.GetFromJsonAsync<List<GroupModel>>("/api/group/GetGroup?createdByUserId" + userId);
            StateHasChanged();
        }
    }
}

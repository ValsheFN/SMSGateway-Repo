using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.ContactGroupList
{
    public partial class ContactGroupList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddContactGroupDialog>("Add Contact Group");
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContactGroups = await _httpClient.GetFromJsonAsync<List<ContactGroupModel>>("/api/contactgroup/GetContactGroup?createdByUserId=" + userId);
            StateHasChanged();
        }

        private async Task Delete(ContactGroupModel contactGroup)
        {
            var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<DeleteContactGroupDialog>("Delete Contact Group", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContactGroups = await _httpClient.GetFromJsonAsync<List<ContactGroupModel>>("/api/contactgroup/GetContactGroup?createdByUserId=" + userId);
            StateHasChanged();
        }

        private async Task Edit(ContactGroupModel contactGroup)
        {
            var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<EditContactGroupDialog>("Edit Contact Group", parameters);
            var result = await dialog.Result;
            var userId = _localStorage.GetItemAsString("user_id");
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContactGroups = await _httpClient.GetFromJsonAsync<List<ContactGroupModel>>("/api/contactgroup/GetContactGroup?createdByUserId=" + userId);
            StateHasChanged();
        }
    }
}

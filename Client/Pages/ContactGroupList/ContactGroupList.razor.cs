using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
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
            ListOfContactGroups = await _httpClient.GetFromJsonAsync<List<ContactGroupModel>>("/api/contactgroup/GetContactGroup");
            StateHasChanged();
        }

        private async Task Delete(ContactGroupModel contactGroup)
        {
            var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<DeleteContactDialog>("Delete Contact", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Edit(ContactGroupModel contactGroup)
        {
            var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<EditContactDialog>("Edit Contact", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }
    }
}

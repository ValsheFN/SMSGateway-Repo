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
            //Check if group exist
            var groups = await _httpClient.GetFromJsonAsync<List<GroupModel>>("/api/group/GetGroup");
            if(groups.Count == 0)
            {
                _snackbar.Add("No group found. Please create group", Severity.Error);
            }
            else
            {
                var dialog = DialogService.Show<AddContactGroupDialog>("Add Contacts to a Group");
                var result = await dialog.Result;
                StateHasChanged();
            } 
        }

        private async Task Delete(ContactGroupModel contactGroup)
        {
            /*var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<DeleteContactDialog>("Delete Contact", parameters);
            var result = await dialog.Result;*/
            StateHasChanged();
        }

        private async Task Edit(ContactGroupModel contactGroup)
        {
            /*var parameters = new DialogParameters { ["ContactGroup"] = contactGroup };

            var dialog = DialogService.Show<EditContactDialog>("Edit Contact", parameters);
            var result = await dialog.Result;*/
            StateHasChanged();
        }
    }
}

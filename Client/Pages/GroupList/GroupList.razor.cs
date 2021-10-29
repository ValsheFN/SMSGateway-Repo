using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.GroupList
{
    public partial class GroupList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddGroupDialog>("Add Group");
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Delete(GroupModel group)
        {
            var parameters = new DialogParameters { ["Group"] = group };

            var dialog = DialogService.Show<DeleteGroupDialog>("Delete Group", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Edit(GroupModel group)
        {
            var parameters = new DialogParameters { ["Group"] = group };

            var dialog = DialogService.Show<EditGroupDialog>("Edit Group", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }
    }
}

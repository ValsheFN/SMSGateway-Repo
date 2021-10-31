using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.TopUpList
{
    public partial class TopUpList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddTopUpDialog>("Add Top Up");
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Approve(TopUpModel topUp)
        {
            /*var parameters = new DialogParameters { ["Top Up"] = topUp };

            var dialog = DialogService.Show<DeleteGroupDialog>("Delete Group", parameters);
            var result = await dialog.Result;
            StateHasChanged();*/
            _navigation.NavigateTo("/topUp");
        }

        private async Task Reject(TopUpModel topUp)
        {
            /*var parameters = new DialogParameters { ["Top Up"] = topUp };

            var dialog = DialogService.Show<EditGroupDialog>("Edit Group", parameters);
            var result = await dialog.Result;
            StateHasChanged();*/
            _navigation.NavigateTo("/topup");
        }
    }
}

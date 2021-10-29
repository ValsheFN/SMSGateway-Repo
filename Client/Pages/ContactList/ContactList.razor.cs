﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.ContactList
{
    public partial class ContactList
    {

        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddContactDialog>("Add Contact");
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Delete(ContactModel contact)
        {
            var parameters = new DialogParameters { ["Contact"] = contact};

            var dialog = DialogService.Show<DeleteContactDialog>("Delete Contact", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Edit(ContactModel contact)
        {
            var parameters = new DialogParameters { ["Contact"] = contact};

            var dialog = DialogService.Show<EditContactDialog>("Edit Contact", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }
    }
}
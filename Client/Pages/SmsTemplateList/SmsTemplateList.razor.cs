using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Threading.Tasks;

namespace SMSGateway.Client.Pages.SmsTemplateList
{
    public partial class SmsTemplateList
    {
        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private async void Create()
        {
            var dialog = DialogService.Show<AddSmsTemplateDialog>("Add Sms Template");
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Delete(SmsTemplateModel template)
        {
            var parameters = new DialogParameters { ["Template"] = template };

            var dialog = DialogService.Show<DeleteSmsTemplateDialog>("Delete Sms Template", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }

        private async Task Edit(SmsTemplateModel template)
        {
            var parameters = new DialogParameters { ["Template"] = template };

            var dialog = DialogService.Show<EditSmsTemplateDialog>("Edit Sms Template", parameters);
            var result = await dialog.Result;
            StateHasChanged();
        }
    }
}

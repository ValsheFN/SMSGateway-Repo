using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;
using SMSGateway.Client.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSGateway.Client.Pages.Dialog
{
    public partial class AddTopUpDialog
    {
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void CreateGroup(decimal topUpValue)
        {
            if(topUpValue <= 0)
            {
                Snackbar.Add("Top up value cannot be equal to or below 0", Severity.Success);
            }
            else
            {
                var authState = _authenticationStateProvider.GetAuthenticationStateAsync();
                //var email = authState.User.FindFirst("sub")?.Value;

                var topUp = new TopUpModel
                {
                    TopUpValue = topUpValue,
                    Requester = ""
                };

                var response = _httpClient.PostAsJsonAsync("/api/topUp/CreateTopUp/", topUp);

                StateHasChanged();
                _navigation.NavigateTo("/topUp");

                Snackbar.Add("Top Up Requested", Severity.Success);
                MudDialog.Close(DialogResult.Ok(topUp.ReferenceId));
            }
        }
    }
}

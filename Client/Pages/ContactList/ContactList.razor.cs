using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SMSGateway.Client.Models;
using SMSGateway.Client.Pages.Dialog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SMSGateway.Client.Pages.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SMSGateway.Client.Pages.ContactList
{
    public partial class ContactList
    {

        [Inject]
        public ILocalStorageService Storage { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        private IEnumerable<ContactModel> ListOfContacts = new List<ContactModel>();

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
                ListOfContacts = await _httpClient.GetFromJsonAsync<List<ContactModel>>("/api/contact/GetContact");
            }
                                                                               
        }

        private async void Create()
        {
            var dialog = DialogService.Show<AddContactDialog>("Add Contact");
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContacts = await _httpClient.GetFromJsonAsync<List<ContactModel>>("/api/contact/GetContact");
            StateHasChanged();
        }

        private async Task Delete(ContactModel contact)
        {
            var parameters = new DialogParameters { ["Contact"] = contact};

            var dialog = DialogService.Show<DeleteContactDialog>("Delete Contact", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContacts = await _httpClient.GetFromJsonAsync<List<ContactModel>>("/api/contact/GetContact");
            StateHasChanged();
        }

        private async Task Edit(ContactModel contact)
        {
            var parameters = new DialogParameters { ["Contact"] = contact};

            var dialog = DialogService.Show<EditContactDialog>("Edit Contact", parameters);
            var result = await dialog.Result;
            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ListOfContacts = await _httpClient.GetFromJsonAsync<List<ContactModel>>("/api/contact/GetContact");
            StateHasChanged();
        }
    }
}

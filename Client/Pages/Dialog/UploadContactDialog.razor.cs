using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using SMSGateway.Client.Models;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using OfficeOpenXml;
using MudBlazor;
using SMSGateway.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;

namespace SMSGateway.Client.Pages.Dialog
{
    public partial class UploadContactDialog
    {
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void DownloadTemplate()
        {
            _navigation.NavigateTo("/ExcelTemplate/sample-contactUpload.xlsx", true);
        }
        
        private async void UploadContact(InputFileChangeEventArgs e)
        {
            isLoading = true;

            var entries = e.GetMultipleFiles();
            var file = entries.FirstOrDefault();
            var extension = entries.FirstOrDefault().Name.Split(".").Last();
            var fileName = entries.FirstOrDefault().Name;

            if (extension != "xlsx")
            {
                Snackbar.Add("File extension should be '.xlsx'", Severity.Error);
            }

            /*List<ContactModel> contacts = new List<ContactModel>();
            await using FileStream fs = new(file, FileMode.Create);
            await browserFile.OpenReadStream().CopyToAsync(fs);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        users.Add(new UserModel { Name = reader.GetValue(0).ToString(), Email = reader.GetValue(1).ToString(), Phone = reader.GetValue(2).ToString() });
                    }
                }
            }*/

            var token = _localStorage.GetItemAsString("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("/api/contact/Import", token);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ExcelImportResponse<ContactModel>>(responseBody);

            if (result.IsSuccess)
            {
                _errorMessage = result.Error;
                MudDialog.Close(DialogResult.Ok(""));
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                _errorMessage = result.Error;
                Snackbar.Add(result.Message, Severity.Error);
            }

            isLoading = false;
        }
    }
}

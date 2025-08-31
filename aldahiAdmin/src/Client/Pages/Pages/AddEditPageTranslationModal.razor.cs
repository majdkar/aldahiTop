using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared;
using FirstCall.Shared.Constants;
using FirstCall.Shared.ViewModels.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Pages
{
    public partial class AddEditPageTranslationModal
    {
        [Parameter]
        public PageTranslationUpdateModel PageTranslationModel { get; set; } = new();

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        private LanguageSelector languageSelector { get; set; }

        private IList<IBrowserFile> _files = new List<IBrowserFile>();
        private FileUploadModel fileUploadModel;

        private bool isProcessing = false;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            var generatedFileName = await UploadFile();
            var FileUrlPath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.PagesFiles.ToString(), generatedFileName);

            PageTranslationModel.File = !String.IsNullOrEmpty(generatedFileName) ? FileUrlPath : "";

            PageTranslationModel.Language = languageSelector.Language;

            var content = HelperMethods.ToJson(PageTranslationModel);
            HttpResponseMessage response;
            if (PageTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.PagesTransaltion, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.PagesTransaltion}/{PageTranslationModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                MudDialog.Close();
            }
            else
            {
                _snackBar.Add("Something went wrong!", Severity.Error);
            }
            isProcessing = false;
        }

        private async void SelectFile(InputFileChangeEventArgs e)
        {
            _files.Clear();
            _files.Add(e.File);

            if (_files.Count > 0)
            {
                fileUploadModel = await HelperMethods.Save(e.File);
                this.StateHasChanged();
            }
        }

        private async Task<string> UploadFile()
        {
            if (_files.Count > 0)
            {
                using var content = new MultipartFormDataContent();
                content.Add
                (content: fileUploadModel.Content, name: "\"file\"", fileName: fileUploadModel.Name);

                var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.PagesFiles}/{(int)Enums.UploadType.File}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.getMessage();
                }
                else
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

    }
}


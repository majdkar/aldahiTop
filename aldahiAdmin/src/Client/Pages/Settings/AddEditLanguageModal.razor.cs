using FirstCall.Client.Helpers;
using FirstCall.Shared.ViewModels.Settings.Languages;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Settings
{
    public partial class AddEditLanguageModal
    {
        [Parameter]
        public LanguageUpdateModel LanguageModel { get; set; } = new();

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        private bool isProcessing = false;

        public void Cancel()
        {
            MudDialog.Cancel();

        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            var content = HelperMethods.ToJson(LanguageModel);
            HttpResponseMessage response;
            if (LanguageModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.Languages, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.Languages}/{LanguageModel.Id}", content);
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

    }
}

using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class AddEditBlockTranslationModal
    {
        [Parameter]
        public BlockTranslationUpdateModel BlockTranslationModel { get; set; } = new();

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        private LanguageSelector languageSelector { get; set; }

        private bool isProcessing = false;

        public void Cancel()
        {
            MudDialog.Cancel();

        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            BlockTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(BlockTranslationModel);
            HttpResponseMessage response;
            if (BlockTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.BlocksTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.BlocksTranslation}/{BlockTranslationModel.Id}", content);
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


using FirstCall.Shared.ViewModels.Blocks;
using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;
using FirstCall.Client.Extensions;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class AddEditBlockCategoryTranslationModal
    {
        [Parameter]
        public BlockCategoryTranslationUpdateModel BlockCategoryTranslationModel { get; set; } = new();

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
            if (string.IsNullOrEmpty(BlockCategoryTranslationModel.Description))
            {
                BlockCategoryTranslationModel.Description = "";
            }
            BlockCategoryTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(BlockCategoryTranslationModel);
            HttpResponseMessage response;
            if (BlockCategoryTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.BlockCategoriesTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.BlockCategoriesTranslation}/{BlockCategoryTranslationModel.Id}", content);
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


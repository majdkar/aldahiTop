using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Menus
{
    public partial class AddEditMenuCategoryTranslationModal
    {
        [Parameter]
        public MenuCategoryTranslationUpdateModel MenuCategoryTranslationModel { get; set; } = new();

        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        public LanguageSelector languageSelector { get; set; }

        private bool isProcessing = false;

        public void Cancel()
        {
            MudDialog.Cancel();

        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            MenuCategoryTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(MenuCategoryTranslationModel);
            HttpResponseMessage response;
            if (MenuCategoryTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.MenuCategoriesTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.MenuCategoriesTranslation}/{MenuCategoryTranslationModel.Id}", content);
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


using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Menus
{
    public partial class AddEditMenuTranslationModal
    {
        [Parameter]
        public MenuTranslationUpdateModel MenuTranslationModel { get; set; } = new();

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
            MenuTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(MenuTranslationModel);
            HttpResponseMessage response;
            if (MenuTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.MenusTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.MenusTranslation}/{MenuTranslationModel.Id}", content);
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


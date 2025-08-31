using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared.ViewModels.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Events
{
    public partial class AddEditEventCategoryTranslationModal
    {
        [Parameter]
        public EventCategoryTranslationUpdateModel EventCategoryTranslationModel { get; set; } = new();

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
            if (string.IsNullOrEmpty(EventCategoryTranslationModel.Description))
            {
                EventCategoryTranslationModel.Description = "";
            }
            EventCategoryTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(EventCategoryTranslationModel);
            HttpResponseMessage response;
            if (EventCategoryTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.EventCategoriesTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.EventCategoriesTranslation}/{EventCategoryTranslationModel.Id}", content);
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

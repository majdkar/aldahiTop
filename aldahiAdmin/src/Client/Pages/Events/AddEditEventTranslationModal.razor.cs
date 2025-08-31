using FirstCall.Client.Helpers;
using FirstCall.Client.Shared.Components;
using FirstCall.Shared.ViewModels.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Events
{
    public partial class AddEditEventTranslationModal
    {
        [Parameter]
        public EventTranslationUpdateModel EventTranslationModel { get; set; } = new();

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
            EventTranslationModel.Language = languageSelector.Language;
            var content = HelperMethods.ToJson(EventTranslationModel);
            HttpResponseMessage response;
            if (EventTranslationModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.EventsTranslation, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.EventsTranslation}/{EventTranslationModel.Id}", content);
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
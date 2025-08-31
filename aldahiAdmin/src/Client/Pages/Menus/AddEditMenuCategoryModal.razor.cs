using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Menus
{
    public partial class AddEditMenuCategoryModal
    {

        [Parameter]
        public MenuCategoryUpdateModel MenuCategoryModel { get; set; } = new();


        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool _rightToLeft = true;
        private string direction = "rtl";

        private bool isProcessing = false;
        protected override async Task OnInitializedAsync()
        {
            _rightToLeft = await _clientPreferenceManager.IsRTL();
            if (_rightToLeft)
                direction = "rtl";
            else
                direction = "ltr";
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();

        }

        private async Task SaveAsync()
        {
            isProcessing = true;
            var content = HelperMethods.ToJson(MenuCategoryModel);
            HttpResponseMessage response;
            if (MenuCategoryModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.MenuCategories, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.MenuCategories}/{MenuCategoryModel.Id}", content);
            }
            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Completed Successful!", Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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


using FirstCall.Client.Extensions;
using FirstCall.Client.Helpers;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.Blocks
{
    public partial class AddEditBlockCategoryModal
    {

        [Parameter]
        public BlockCategoryUpdateModel BlockCategoryModel { get; set; } = new();


        private bool _rightToLeft = true;
        private string direction = "rtl";
        private bool isProcessing = false;

        private HubConnection HubConnection { get; set; }
        [CascadingParameter]

        private MudDialogInstance MudDialog { get; set; }

        private List<string> BlockTypes = new List<string> {
            "Blog" ,
            "Link" ,
            "News" ,
            "Event" ,
            "Activity" ,
            "Article" ,
            "Photo Gallery" ,
            "Video Gallery" ,
            "Home Slider" ,
        };

        public void Cancel()
        {
            MudDialog.Cancel();

        }


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

        private async Task SaveAsync()
        {
            isProcessing = true;
            if (string.IsNullOrEmpty(BlockCategoryModel.Description))
            {
                BlockCategoryModel.Description = "";
            }
            if (string.IsNullOrEmpty(BlockCategoryModel.EnglishDescription))
            {
                BlockCategoryModel.EnglishDescription = "";
            }
            var content = HelperMethods.ToJson(BlockCategoryModel);
            HttpResponseMessage response;
            if (BlockCategoryModel.Id == 0)
            {
                response = await _httpClient.PostAsync(EndPoints.BlockCategories, content);
            }
            else
            {
                response = await _httpClient.PutAsync($"{EndPoints.BlockCategories}/{BlockCategoryModel.Id}", content);
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


using Blazored.FluentValidation;
using FirstCall.Application.Features.Brands.Commands.AddEdit;
using FirstCall.Application.Requests;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Brand;
using FirstCall.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class AddEditBrandModal
    {
        [Inject] private IBrandManager BrandManager { get; set; }

        [Parameter] public AddEditBrandCommand AddEditBrandModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private bool _rightToLeft = true;
        private string direction = "rtl";
        private bool isProcessing = false;


        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }






        private async Task SaveAsync()
        {

            isProcessing = true;
            var response = await BrandManager.SaveAsync(AddEditBrandModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
            isProcessing = false;
        }





        protected override async Task OnInitializedAsync()
        {
            _rightToLeft = await _clientPreferenceManager.IsRTL();
            if (_rightToLeft)
                direction = "rtl";
            else
                direction = "ltr";
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();


        }




        private async Task LoadImageAsync()
        {
            var data = await BrandManager.GetbrandImageAsync(AddEditBrandModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditBrandModel.ImageDataURL = imageData;

                }
            }
        }
        private void DeleteAsync()
        {
            AddEditBrandModel.ImageDataURL = null;
            AddEditBrandModel.UploadRequest = new UploadRequest();
        }
        private IBrowserFile _file;
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditBrandModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditBrandModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Brand, Extension = extension };
            }
        }
    }
}
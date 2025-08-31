
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Application.Requests;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using FirstCall.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.JSInterop;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class AddEditProductCategoryModal
    {
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
        [Parameter] public AddEditProductCategoryCommand AddEditProductCategoryModel { get; set; } = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllProductCategoriesResponse> _ProductCategories = new();
        private bool _loaded = false;
        private bool _processing = false;
        public void Cancel()
        {
            MudDialog.Cancel();
        }
       

        private async Task SaveAsync()
        {
            _processing = true;
            var response = await ProductCategoryManager.SaveAsync(AddEditProductCategoryModel);
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
            _processing = false;
        }



        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await LoadProductCategoriesAsync();

        }

        private async Task LoadProductCategoriesAsync()
        {

            //var data = await ProductCategoryManager.GetAllByTypeAsync(AddEditProductCategoryModel.Type);
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _ProductCategories = data.Data;
            }
        }

        private async Task LoadImageAsync()
        {
            var data = await ProductCategoryManager.GetProductCategoryImageAsync(AddEditProductCategoryModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditProductCategoryModel.ImageDataURL = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            AddEditProductCategoryModel.ImageDataURL = null;
            AddEditProductCategoryModel.UploadRequest = new UploadRequest();
        }
        private IBrowserFile _imageFile;
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditProductCategoryModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductCategoryModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Person, Extension = extension };
            }
        }
        private async void OnParentChanged(int value)
        {

            AddEditProductCategoryModel.ParentCategoryId = value;

           
        }
        private async Task<IEnumerable<int>> SearchCategories(string value)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _ProductCategories.Select(x => x.Id);

            return _ProductCategories.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}

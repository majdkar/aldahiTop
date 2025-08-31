using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Requests;
using FirstCall.Application.Requests.Identity;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Brand;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using FirstCall.Client.Infrastructure.Managers.ProductComponents;
using FirstCall.Application.Features.ProductComponents.Commands.AddEdit;

namespace FirstCall.Client.Pages.Products
{
    public partial class AddEditCompanyProductComponent
    {
        [Inject] private IProductComponentManager ProductComponentManager { get; set; }

        [Parameter] public int ProductComponentId { get; set; } = 0;
        [Parameter] public int ProductId { get; set; } = 0;

        private AddEditCompanyProductComponentCommand AddEditCompanyProductComponentModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;




        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });



        TextEditorConfig editorDescriptionAboutEn = new TextEditorConfig("#editorEn");
     



        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
            //MudDialog.Cancel();

        }
        private async Task LoadDataAsync()
        {
            await Task.WhenAll(
             LoadProductComponentDetails()
          );
        }




        private async Task LoadProductComponentDetails()
        {
            if (ProductComponentId != 0)
            {
                var data = await ProductComponentManager.GetByIdAsync(ProductComponentId);
                if (data.Succeeded)
                {
                    var ProductComponent = data.Data;
                    AddEditCompanyProductComponentModel = new AddEditCompanyProductComponentCommand
                    {
                        Id = ProductComponent.Id,
                        NameAr = ProductComponent.NameAr,
                        NameEn = ProductComponent.NameEn,
                        DescriptionAboutEn = ProductComponent.DescriptionAboutEn,
                        ProductId = ProductComponent.ProductId,
                        ProductComponentImageUrl=ProductComponent.ProductComponentImageUrl,
                        ProductComponentImageUrl2=ProductComponent.ProductComponentImageUrl2,
                        ProductComponentImageUrl3=ProductComponent.ProductComponentImageUrl3,
                        ProductComponentImageUrl4=ProductComponent.ProductComponentImageUrl4,
                        ProductComponentImageUrl5=ProductComponent.ProductComponentImageUrl5,
                        ProductComponentImageUrl1=ProductComponent.ProductComponentImageUrl1,
                        Order = ProductComponent.Order,
                    };
                }
            }
            else
            {
                AddEditCompanyProductComponentModel.Id = 0;
                AddEditCompanyProductComponentModel.ProductId = ProductId;
            }

        }


        private async Task SaveAsync()
        {


        
       

            var response = await ProductComponentManager.SaveForCompanyProfileAsync(AddEditCompanyProductComponentModel);
            if (response.Succeeded)
            {
                
                _snackBar.Add(response.Messages[0], Severity.Success);
                 Cancel();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
           
        }
        private IBrowserFile _imageFile;
        private IBrowserFile _imageFile1;
        private IBrowserFile _imageFile2;
        private IBrowserFile _imageFile3;
        private IBrowserFile _imageFile4;
        private IBrowserFile _imageFile5;


        private async Task UploadProductComponentImage(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        } 
        private async Task UploadProductComponentImage1(InputFileChangeEventArgs e)
        {
            _imageFile1 = e.File;
            if (_imageFile1 != null)
            {
                var extension = Path.GetExtension(_imageFile1.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl1 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest1 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        } 
        private async Task UploadProductComponentImage2(InputFileChangeEventArgs e)
        {
            _imageFile2 = e.File;
            if (_imageFile2 != null)
            {
                var extension = Path.GetExtension(_imageFile2.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        } 
        private async Task UploadProductComponentImage3(InputFileChangeEventArgs e)
        {
            _imageFile3 = e.File;
            if (_imageFile3 != null)
            {
                var extension = Path.GetExtension(_imageFile3.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }  private async Task UploadProductComponentImage4(InputFileChangeEventArgs e)
        {
            _imageFile4 = e.File;
            if (_imageFile4 != null)
            {
                var extension = Path.GetExtension(_imageFile4.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl4 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest4 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }  private async Task UploadProductComponentImage5(InputFileChangeEventArgs e)
        {
            _imageFile5 = e.File;
            if (_imageFile5 != null)
            {
                var extension = Path.GetExtension(_imageFile5.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyProductComponentModel.ProductComponentImageUrl5 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductComponentModel.UploadRequest5 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }
        private void DeleteProductComponentImageAsync()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl = null;
            AddEditCompanyProductComponentModel.UploadRequest = new UploadRequest();
        }  private void DeleteProductComponentImageAsync1()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl1 = null;
            AddEditCompanyProductComponentModel.UploadRequest1 = new UploadRequest();
        }  private void DeleteProductComponentImageAsync2()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl2 = null;
            AddEditCompanyProductComponentModel.UploadRequest2 = new UploadRequest();
        }  private void DeleteProductComponentImageAsync3()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl3 = null;
            AddEditCompanyProductComponentModel.UploadRequest3 = new UploadRequest();
        }  private void DeleteProductComponentImageAsync4()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl4 = null;
            AddEditCompanyProductComponentModel.UploadRequest4 = new UploadRequest();
        }  private void DeleteProductComponentImageAsync5()
        {
            AddEditCompanyProductComponentModel.ProductComponentImageUrl5 = null;
            AddEditCompanyProductComponentModel.UploadRequest5 = new UploadRequest();
        }
    }
}

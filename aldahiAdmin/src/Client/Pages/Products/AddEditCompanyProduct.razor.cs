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
using FirstCall.Client.Infrastructure.Managers.Products;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Season;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Kind;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Group;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Warehouses;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Shared.Constants.Products;

namespace FirstCall.Client.Pages.Products
{
    public partial class AddEditCompanyProduct
    {
        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
        [Inject] private ISeasonManager SeasonManager { get; set; }
        [Inject] private IGroupManager GroupManager { get; set; }
        [Inject] private IKindManager KindManager { get; set; }
        [Inject] private IWarehousesManager WarehousesManager { get; set; }

        [Parameter] public int ProductId { get; set; } = 0;
        [Parameter] public string ProductType { get; set; }

        private AddEditCompanyProductCommand AddEditCompanyProductModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private List<GetAllSeasonsResponse> _allCategories = new();
        private List<GetAllGroupsResponse> _groups = new();
        private List<GetAllKindsResponse> _allKinds = new();
        private List<GetAllWarehousessResponse> _allWarehouses = new();
        private List<GetAllProductCategoriesResponse> _productCategors = new();
     

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });


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
        }
        private async Task LoadDataAsync()
        {
            await Task.WhenAll(
          LoadAllProductCategories(),
          LoadAllSeasons(),
          LoadAllGroups(),
          LoadAllWarehouses(),
          LoadProductDetails(),
          LoadAllProductKinds()
          );
        }

        private async Task LoadAllProductCategories()
        {
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _productCategors = data.Data;

            }
        }
        private async Task LoadAllSeasons()
        {
            var data = await SeasonManager.GetAllAsync();
            if (data.Succeeded)
            {
                _allCategories = data.Data;

            }
        }     
        
        private async Task LoadAllGroups()
        {
            var data = await GroupManager.GetAllAsync();
            if (data.Succeeded)
            {
                _groups = data.Data;

            }
        }
        private async Task LoadAllWarehouses()
        {
            var data = await WarehousesManager.GetAllAsync();
            if (data.Succeeded)
            {
                _allWarehouses = data.Data;

            }
        }
        private async Task LoadAllProductKinds()
        {
            var data = await KindManager.GetAllAsync();
            if (data.Succeeded)
            {
                _allKinds = data.Data;

            }
        }


 


    

        private async Task LoadProductDetails()
        {
            if (ProductId != 0)
            {
                var data = await ProductManager.GetByIdAsync(ProductId);
                if (data.Succeeded)
                {
                    var product = data.Data;
                    AddEditCompanyProductModel = new AddEditCompanyProductCommand
                    {
                        Id = product.Id,
                        NameAr = product.NameAr,
                        NameEn = product.NameEn,
             
                        Code = product.Code,
                        Price = product.Price,
                        Order = product.Order,
                        ProductImageUrl=product.ProductImageUrl,
                        ProductImageUrl2=product.ProductImageUrl2,
                        ProductImageUrl3=product.ProductImageUrl3,
                        ProductImageUrl4=product.ProductImageUrl4,
                        SeasonId = product.SeasonId,
                        KindId = product.KindId,
                        Colors = product.Colors,
                        Sizes = product.Sizes,
                        Qty = product.Qty,
                        WarehousesId  = product.WarehousesId,
                        PackageNumber = product.PackageNumber,
                         ProductCategoryId = product.ProductCategoryId,
                          GroupId = product.GroupId,
                    };
                 
                }
            }
            else
            {
                AddEditCompanyProductModel.Id = 0;
            }

        }


        private async Task SaveAsync()
        {
            AddEditCompanyProductModel.Type = ProductType;
            var response = await ProductManager.SaveForCompanyProfileAsync(AddEditCompanyProductModel);
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
        private IBrowserFile _imageFile2;
        private IBrowserFile _imageFile3;
        private IBrowserFile _imageFile4;
        private async Task UploadProductImage(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[_imageFile.Size];
                await _imageFile.OpenReadStream(1000000000).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }
        private void DeleteProductImageAsync()
        {
            AddEditCompanyProductModel.ProductImageUrl = null;
            AddEditCompanyProductModel.UploadRequest = new UploadRequest();
        }

        private async Task UploadProductImage2(InputFileChangeEventArgs e)
        {
            _imageFile2 = e.File;
            if (_imageFile2 != null)
            {
                var extension = Path.GetExtension(_imageFile2.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[_imageFile2.Size];
                await _imageFile2.OpenReadStream(1000000000).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequest2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }
        private void DeleteProductImage2Async()
        {
            AddEditCompanyProductModel.ProductImageUrl2 = null;
            AddEditCompanyProductModel.UploadRequest2 = new UploadRequest();
        }
        private async Task UploadProductImage3(InputFileChangeEventArgs e)
        {
            _imageFile3 = e.File;
            if (_imageFile3 != null)
            {
                var extension = Path.GetExtension(_imageFile3.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[_imageFile3.Size];
                await _imageFile3.OpenReadStream(1000000000).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequest3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }
        private void DeleteProductImage3Async()
        {
            AddEditCompanyProductModel.ProductImageUrl3 = null;
            AddEditCompanyProductModel.UploadRequest3 = new UploadRequest();
        }   
        
        private async Task UploadProductImage4(InputFileChangeEventArgs e)
        {
            _imageFile4 = e.File;
            if (_imageFile4 != null)
            {
                var extension = Path.GetExtension(_imageFile4.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[_imageFile4.Size];
                await _imageFile4.OpenReadStream(1000000000).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl4 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequest4 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }
        private void DeleteProductImage4Async()
        {
            AddEditCompanyProductModel.ProductImageUrl4 = null;
            AddEditCompanyProductModel.UploadRequest4 = new UploadRequest();
        }
    }
}

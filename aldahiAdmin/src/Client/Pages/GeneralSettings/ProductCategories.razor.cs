using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged;
using FirstCall.Application.Requests.ProductCategories;
using FirstCall.Application.Requests.Products;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Shared.Constants.Products;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class ProductCategories
    {
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
        //[Parameter] public string Type { get; set; }

        private IEnumerable<GetAllPagedProductCategoriesResponse> _pagedData;
        private MudTable<GetAllPagedProductCategoriesResponse> _table;
        public int CategoryId { get; set; } = 0;
        [Parameter] public string CategoryName { get; set; } = "";
        private List<GetAllProductCategoriesResponse> _parentCategories = new();
        private List<GetAllProductCategoriesResponse> _subCategories = new();
        private List<GetAllProductCategoriesResponse> _allCategories = new();
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductCategories;
        private bool _canEditProductCategories;
        private bool _canDeleteProductCategories;
        private bool _canExportProductCategories;
        private bool _canSearchProductCategories;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProductCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProductCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;
            _canExportProductCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            _canSearchProductCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            await LoadAllProductCategories();
            _loaded = true;
           
        }

        private async Task<TableData<GetAllPagedProductCategoriesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductCategoriesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }
        public  void Back()
        {
            //  await _jsRuntime.InvokeVoidAsync("history.back", -1);
            _navigationManager.NavigateTo($"/home");

        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedProductCategoriesRequest {Type = ProductTypesEnum.B2C.ToString(), PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            //var response = await ProductCategoryManager.GetPagedByTypeAsync(request);
            var response = await ProductCategoryManager.GetAllCategorySonsAsync(request, CategoryId);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }


        private async Task LoadAllProductCategories()
        {
            //_allCategories.Clear();
            var data = await ProductCategoryManager.GetAllByTypeAsync(ProductTypesEnum.B2C.ToString());
            if (data.Succeeded)
            {
                _allCategories = data.Data;
                _parentCategories = _allCategories.Where(x => (x.ParentCategoryId == null) || (x.ParentCategoryId == 0)).ToList();

                string categoryName = "";
                if ((CategoryId > 0) && (_allCategories.FirstOrDefault(x => x.Id == CategoryId) is not null))
                {
                    categoryName = _allCategories.FirstOrDefault(x => x.Id == CategoryId).NameAr;
                    var categoryParentId = _allCategories.FirstOrDefault(x => x.Id == CategoryId).ParentCategoryId;
                    while (_allCategories.FirstOrDefault(x => x.Id == categoryParentId) is not null)
                    {
                        categoryName = _allCategories.FirstOrDefault(x => x.Id == categoryParentId).NameAr + "-" + categoryName;
                        categoryParentId = _allCategories.FirstOrDefault(x => x.Id == categoryParentId).ParentCategoryId;
                    }
                }
                CategoryName = categoryName;



            }
        }
        private async Task AddSubCategory()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(AddEditProductCategoryModal.AddEditProductCategoryModel), new AddEditProductCategoryCommand
            {
                Id = 0,

                ParentCategoryId = CategoryId,
                Type=ProductTypesEnum.B2C.ToString(),
            });

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductCategoryModal>(_localizer["Create"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
        }

        private void InvokeSons(int parentcategoryId)
        {
            //_navigationManager.NavigateTo($"general-settings/Product-Categories/{id}", true);
            CategoryId = parentcategoryId;
            string categoryName = "";
            if ((CategoryId > 0) && (_allCategories.FirstOrDefault(x => x.Id == CategoryId) is not null))
            {
                categoryName = _allCategories.FirstOrDefault(x => x.Id == CategoryId).NameAr;
                var categoryParentId = _allCategories.FirstOrDefault(x => x.Id == CategoryId).ParentCategoryId;
                while (_allCategories.FirstOrDefault(x => x.Id == categoryParentId) is not null)
                {
                    categoryName = _allCategories.FirstOrDefault(x => x.Id == categoryParentId).NameAr + "-" + categoryName;
                    categoryParentId = _allCategories.FirstOrDefault(x => x.Id == categoryParentId).ParentCategoryId;
                }
            }
            CategoryName = categoryName;
            //CategoryLevel++;
            _table.ReloadServerData();
            // loaded = false;
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await ProductCategoryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ProductCategories).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["ProductCategories exported"]
                    : _localizer["Filtered ProductCategories exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var ProductCategory = _pagedData.FirstOrDefault(c => c.Id == id);
                if (ProductCategory != null)
                {
                    parameters.Add(nameof(AddEditProductCategoryModal.AddEditProductCategoryModel), new AddEditProductCategoryCommand
                    {
                        Id = ProductCategory.Id,
                        NameEn = ProductCategory.NameEn,
                        NameAr = ProductCategory.NameAr,
                        DescriptionEn = ProductCategory.DescriptionEn,
                        DescriptionAr = ProductCategory.DescriptionAr,
                        Order = ProductCategory.Order,
                        ParentCategoryId = Convert.ToInt32(ProductCategory.ParentCategoryId),
                        Type = ProductTypesEnum.B2C.ToString(),
                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditProductCategoryModal.AddEditProductCategoryModel), new AddEditProductCategoryCommand
                {
                    Id = 0,
                    Type = ProductTypesEnum.B2C.ToString(),
                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductCategoryModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ProductCategoryManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}

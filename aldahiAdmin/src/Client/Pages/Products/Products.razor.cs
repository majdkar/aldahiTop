using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Requests.Products;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.Products;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace FirstCall.Client.Pages.Products
{
    public partial class Products
    {
        [Inject] private IProductManager ProductManager { get; set; }

        [Parameter]  public string ProductType { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        public string ProductName { get; set; }
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private MudTable<GetAllPagedProductsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProduct;
        private bool _canEditProduct;
        private bool _canDeleteProduct;
        private bool _canExportProduct;
        private bool _canSearchProduct;
        private bool _loaded;


  

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;
            _canExportProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Export)).Succeeded;
            _canSearchProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

          
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_loaded && _table is not null)
            {
                ProductName = null;
                FromPrice = 0;
                ToPrice = 0;
                _searchString = string.Empty;

                await _table.ReloadServerData();
            }
        }



        private async Task FilterData()
        {
            await _table.ReloadServerData();
        }

        private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            
         
            var response = await ProductManager.GetAllPagedSearchProductAsync(request,ProductName, FromPrice, ToPrice,ProductType);
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

        private void OnSearch(string text)
        {
            ProductName = null;
        FromPrice = 0;

         ToPrice = 0;
        _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await ProductManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Product).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Product exported"]
                    : _localizer["Filtered Product exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToDetails(int ProductId)
        {
            _navigationManager.NavigateTo($"/Product-details/{ProductId}/{ProductType}");
        } 
        
        private void RedirectToComponents(int ProductId,string ProductName)
        {
            _navigationManager.NavigateTo($"/ProductComponents/{ProductId}/{ProductName}");
        }

        private string RedirectToViewDetails(int ProductId)
        {
           return $"/view-details/{ProductId}";
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
                var response = await ProductManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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

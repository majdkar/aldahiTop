using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Stocks.Commands.AddEdit;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Stock;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Permission;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using FirstCall.Client.Extensions;
using System.Linq;
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Brand;
using FirstCall.Application.Requests.Stocks;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Stocks
    {
        [Inject] private IStockManager StockManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllStocksResponse> _StockList = new();
        private GetAllStocksResponse _Stock = new();
        private IEnumerable<GetAllStocksResponse> _pagedData;
        private MudTable<GetAllStocksResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateStocks;
        private bool _canEditStocks;
        private bool _canDeleteStocks;
        private bool _canExportStocks;
        private bool _canSearchStocks;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stocks.Create)).Succeeded;
            _canEditStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stocks.Edit)).Succeeded;
            _canDeleteStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stocks.Delete)).Succeeded;
            _canExportStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stocks.Export)).Succeeded;
            _canSearchStocks = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Stocks.Search)).Succeeded;

            await GetStocksAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<GetAllStocksResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllStocksResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedStockRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await StockManager.GetAllPagedAsync(request);
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
        private async Task GetStocksAsync()
        {
            var response = await StockManager.GetAllAsync();
            if (response.Succeeded)
            {
                _StockList = response.Data.ToList();
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
            _searchString = text;
            _table.ReloadServerData();
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await StockManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

     
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Stock = _StockList.FirstOrDefault(c => c.Id == id);
                if (_Stock != null)
                {
                    parameters.Add(nameof(AddEditStockModal.AddEditStockModel), new AddEditStockCommand
                    {
                        Id = id,
                        NameAr = _Stock.NameAr,
                        NameEn = _Stock.NameEn,
                        ProductId = _Stock.ProductId,
                        WarehousesId = _Stock.WarehousesId,
                        Quantity = _Stock.Quantity,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditStockModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _Stock = new GetAllStocksResponse();
            _table.ReloadServerData();
        }

        private bool Search(GetAllStocksResponse Stock)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Stock.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Stock.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            /**/
            return false;
        }
    }
}

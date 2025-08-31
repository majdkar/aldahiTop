using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Warehouses;
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
using FirstCall.Application.Requests.Warehousess;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Warehousess
    {
        [Inject] private IWarehousesManager WarehousesManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllWarehousessResponse> _WarehousesList = new();
        private GetAllWarehousessResponse _Warehouses = new();
        private IEnumerable<GetAllWarehousessResponse> _pagedData;
        private MudTable<GetAllWarehousessResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateWarehousess;
        private bool _canEditWarehousess;
        private bool _canDeleteWarehousess;
        private bool _canExportWarehousess;
        private bool _canSearchWarehousess;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateWarehousess = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Create)).Succeeded;
            _canEditWarehousess = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Edit)).Succeeded;
            _canDeleteWarehousess = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Delete)).Succeeded;
            _canExportWarehousess = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Export)).Succeeded;
            _canSearchWarehousess = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Warehouses.Search)).Succeeded;

            await GetWarehousessAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<GetAllWarehousessResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllWarehousessResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedWarehousesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await WarehousesManager.GetAllPagedAsync(request);
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
        private async Task GetWarehousessAsync()
        {
            var response = await WarehousesManager.GetAllAsync();
            if (response.Succeeded)
            {
                _WarehousesList = response.Data.ToList();
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
                var response = await WarehousesManager.DeleteAsync(id);
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
                _Warehouses = _WarehousesList.FirstOrDefault(c => c.Id == id);
                if (_Warehouses != null)
                {
                    parameters.Add(nameof(AddEditWarehousesModal.AddEditWarehousesModel), new AddEditWarehousesCommand
                    {
                        Id = id,
                        NameAr = _Warehouses.NameAr,
                        NameEn = _Warehouses.NameEn,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditWarehousesModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _Warehouses = new GetAllWarehousessResponse();
            _table.ReloadServerData();
        }

        private bool Search(GetAllWarehousessResponse Warehouses)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Warehouses.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Warehouses.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            /**/
            return false;
        }
    }
}

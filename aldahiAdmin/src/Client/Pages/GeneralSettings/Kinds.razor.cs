using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Kind;
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
using FirstCall.Application.Requests.Kinds;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Kinds
    {
        [Inject] private IKindManager KindManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllKindsResponse> _KindList = new();
        private GetAllKindsResponse _Kind = new();
        private IEnumerable<GetAllKindsResponse> _pagedData;
        private MudTable<GetAllKindsResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateKinds;
        private bool _canEditKinds;
        private bool _canDeleteKinds;
        private bool _canExportKinds;
        private bool _canSearchKinds;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateKinds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Kinds.Create)).Succeeded;
            _canEditKinds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Kinds.Edit)).Succeeded;
            _canDeleteKinds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Kinds.Delete)).Succeeded;
            _canExportKinds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Kinds.Export)).Succeeded;
            _canSearchKinds = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Kinds.Search)).Succeeded;

            await GetKindsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<GetAllKindsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllKindsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedKindRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await KindManager.GetAllPagedAsync(request);
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
        private async Task GetKindsAsync()
        {
            var response = await KindManager.GetAllAsync();
            if (response.Succeeded)
            {
                _KindList = response.Data.ToList();
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
                var response = await KindManager.DeleteAsync(id);
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
                _Kind = _KindList.FirstOrDefault(c => c.Id == id);
                if (_Kind != null)
                {
                    parameters.Add(nameof(AddEditKindModal.AddEditKindModel), new AddEditKindCommand
                    {
                        Id = id,
                        NameAr = _Kind.NameAr,
                        NameEn = _Kind.NameEn,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditKindModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _Kind = new GetAllKindsResponse();
            _table.ReloadServerData();
        }

        private bool Search(GetAllKindsResponse Kind)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Kind.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Kind.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            /**/
            return false;
        }
    }
}

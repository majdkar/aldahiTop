using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Groups.Commands.AddEdit;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Group;
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
using FirstCall.Application.Requests.Groups;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Groups
    {
        [Inject] private IGroupManager GroupManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllGroupsResponse> _GroupList = new();
        private GetAllGroupsResponse _Group = new();
        private IEnumerable<GetAllGroupsResponse> _pagedData;
        private MudTable<GetAllGroupsResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateGroups;
        private bool _canEditGroups;
        private bool _canDeleteGroups;
        private bool _canExportGroups;
        private bool _canSearchGroups;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateGroups = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Groups.Create)).Succeeded;
            _canEditGroups = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Groups.Edit)).Succeeded;
            _canDeleteGroups = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Groups.Delete)).Succeeded;
            _canExportGroups = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Groups.Export)).Succeeded;
            _canSearchGroups = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Groups.Search)).Succeeded;

            await GetGroupsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<GetAllGroupsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllGroupsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedGroupRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await GroupManager.GetAllPagedAsync(request);
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
        private async Task GetGroupsAsync()
        {
            var response = await GroupManager.GetAllAsync();
            if (response.Succeeded)
            {
                _GroupList = response.Data.ToList();
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
                var response = await GroupManager.DeleteAsync(id);
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
                _Group = _GroupList.FirstOrDefault(c => c.Id == id);
                if (_Group != null)
                {
                    parameters.Add(nameof(AddEditGroupModal.AddEditGroupModel), new AddEditGroupCommand
                    {
                        Id = id,
                        NameAr = _Group.NameAr,
                        NameEn = _Group.NameEn,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditGroupModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _Group = new GetAllGroupsResponse();
            _table.ReloadServerData();
        }

        private bool Search(GetAllGroupsResponse Group)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Group.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Group.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            /**/
            return false;
        }
    }
}

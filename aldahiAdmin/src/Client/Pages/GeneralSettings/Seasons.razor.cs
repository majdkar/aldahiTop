using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Season;
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
using FirstCall.Application.Requests.Seasons;

namespace FirstCall.Client.Pages.GeneralSettings
{
    public partial class Seasons
    {
        [Inject] private ISeasonManager SeasonManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllSeasonsResponse> _SeasonList = new();
        private GetAllSeasonsResponse _Season = new();
        private IEnumerable<GetAllSeasonsResponse> _pagedData;
        private MudTable<GetAllSeasonsResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateSeasons;
        private bool _canEditSeasons;
        private bool _canDeleteSeasons;
        private bool _canExportSeasons;
        private bool _canSearchSeasons;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSeasons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Seasons.Create)).Succeeded;
            _canEditSeasons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Seasons.Edit)).Succeeded;
            _canDeleteSeasons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Seasons.Delete)).Succeeded;
            _canExportSeasons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Seasons.Export)).Succeeded;
            _canSearchSeasons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Seasons.Search)).Succeeded;

            await GetSeasonsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task<TableData<GetAllSeasonsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllSeasonsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedSeasonRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await SeasonManager.GetAllPagedAsync(request);
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
        private async Task GetSeasonsAsync()
        {
            var response = await SeasonManager.GetAllAsync();
            if (response.Succeeded)
            {
                _SeasonList = response.Data.ToList();
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
                var response = await SeasonManager.DeleteAsync(id);
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
                _Season = _SeasonList.FirstOrDefault(c => c.Id == id);
                if (_Season != null)
                {
                    parameters.Add(nameof(AddEditSeasonModal.AddEditSeasonModel), new AddEditSeasonCommand
                    {
                        Id = id,
                        NameAr = _Season.NameAr,
                        NameEn = _Season.NameEn,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditSeasonModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _Season = new GetAllSeasonsResponse();
            _table.ReloadServerData();
        }

        private bool Search(GetAllSeasonsResponse Season)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Season.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Season.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            /**/
            return false;
        }
    }
}

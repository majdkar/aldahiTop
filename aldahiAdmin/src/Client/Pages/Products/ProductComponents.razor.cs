using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Requests.Products;
using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.ProductComponents;
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
    public partial class ProductComponents
    {
        [Inject] private IProductComponentManager ProductComponentManager { get; set; }

        [Parameter] public string ProductName { get; set; } = "";
        [Parameter] public int ProductId { get; set; } = 0;

        [CascadingParameter] private HubConnection HubConnection { get; set; }

     

        private IEnumerable<GetAllPagedProductComponentsResponse> _pagedData;
        private MudTable<GetAllPagedProductComponentsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductComponent;
        private bool _canEditProductComponent;
        private bool _canDeleteProductComponent;
        private bool _canExportProductComponent;
        private bool _canSearchProductComponent;
        private bool _loaded;


  

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductComponent = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductComponents.Create)).Succeeded;
            _canEditProductComponent = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductComponents.Edit)).Succeeded;
            _canDeleteProductComponent = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductComponents.Delete)).Succeeded;
            _canExportProductComponent = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductComponents.Export)).Succeeded;
            _canSearchProductComponent = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductComponents.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

          
        }



      

        private async Task FilterData()
        {
            await _table.ReloadServerData();
        }

        private async Task<TableData<GetAllPagedProductComponentsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductComponentsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedProductComponentsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            
         
            var response = await ProductComponentManager.GetAllPagedProductIdAsync(request,ProductId);
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
        
        _searchString = text;
            _table.ReloadServerData();
        }

   
        private void RedirectToDetails(int ProductComponentId)
        {
            _navigationManager.NavigateTo($"/ProductComponent-details/{ProductComponentId}/{ProductId}");
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
                var response = await ProductComponentManager.DeleteAsync(id);
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

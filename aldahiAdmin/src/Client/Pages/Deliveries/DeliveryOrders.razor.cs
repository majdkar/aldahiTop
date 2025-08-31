using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Application.Requests.Deliveries.DeliveryOrders;
using FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder;
using FirstCall.Shared.Constants.Application;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class DeliveryOrders
    {
        [Inject] private IDeliveryOrderManager DeliveryOrderManager { get; set; }

        private IEnumerable<GetAllDeliveryOrdersResponse> _pagedData;
        private MudTable<GetAllDeliveryOrdersResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";

        private GetAllDeliveryOrdersResponse _deliveryOrder = new();
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        [Parameter] public string Status { get; set; }


        private ClaimsPrincipal _currentUser;
        private bool _canCreateDeliveryOrders;
        private bool _canEditDeliveryOrders;
        private bool _canDeleteDeliveryOrders;
        private bool _canExportDeliveryOrders;
        private bool _canSearchDeliveryOrders;
        private bool _canAddDeliveryPrice;
        private bool _loaded;

        public bool IsSearchAdvanced { get; set; } = false;
        //private string categoryId = "0"; // Query string parameter
        public string ClientName { set; get; } = "";
        public decimal? StartPrice { set; get; } = 0;
        public decimal? EndPrice { set; get; } = 0;
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? StartDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; }
        public int CategoryId { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDeliveryOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Create)).Succeeded;
            _canEditDeliveryOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Edit)).Succeeded;
            _canDeleteDeliveryOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Delete)).Succeeded;
            _canExportDeliveryOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Export)).Succeeded;
            _canSearchDeliveryOrders = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Search)).Succeeded;

            _loaded = true;

        }

        private async Task<TableData<GetAllDeliveryOrdersResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllDeliveryOrdersResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedDeliveryOrdersRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings, Status = Status };
            var response = await DeliveryOrderManager.GetAllByStatusAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
                if (IsSearchAdvanced == true)
                {
                    if (!string.IsNullOrEmpty(ClientName))
                    {
                        _pagedData = _pagedData.Where(e => e.ClientName.ToLower().Contains(ClientName.ToLower())).ToList();
                    }
                    if (StartPrice.HasValue)
                    {
                        _pagedData = _pagedData.Where(x => x.TotalPrice >= StartPrice).ToList();
                    }
                    if (EndPrice.HasValue)
                    {
                        _pagedData = _pagedData.Where(x => x.TotalPrice <= EndPrice).ToList();
                    }

                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private string RedirectToViewDetails(int id)
        {
            return $"/view-details-DeliveryOrders/{id}";
        }
       
        private void OnSearch(string text)
        {
            IsSearchAdvanced = false;

            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content1"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DeliveryOrderManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
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

        private async Task ExportToExcel()
        {
            var response = await DeliveryOrderManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(DeliveryOrders).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["DeliveryOrders exported"]
                    : _localizer["Filtered DeliveryOrders exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Reset()
        {
            _deliveryOrder = new GetAllDeliveryOrdersResponse();
            await _table.ReloadServerData();
        }

        private async Task Accept(int id)
        {
            string deleteContent = _localizer["Confirm Operation"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DeliveryOrderManager.AccceptOrderRequestAsync(id);
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

        private async Task Refuse(int id)
        {
            string deleteContent = _localizer["Confirm Operation"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DeliveryOrderManager.RefuseAsync(id);
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

        private async Task RedirectToAddEdit(int id)
        {
            _navigationManager.NavigateTo($"/order-details/{id}");
            await _table.ReloadServerData();

        }

        private void Details(int id)
        {
            _navigationManager.NavigateTo($"/DetailsOrder/{id}");
        }


        private bool Search(GetAllDeliveryOrdersResponse deliveryOrder)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            

            
            return false;
        }

        private async Task SearchAdvance()
        {
            IsSearchAdvanced = true;
            await _table.ReloadServerData();
        }


    }
}

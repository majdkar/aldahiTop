
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using FirstCall.Shared.Constants.Permission;
using FirstCall.Client.Extensions;

using FirstCall.Shared.Constants.Application;
using FirstCall.Client.Infrastructure.Managers.Products;
using FirstCall.Client.Infrastructure.Managers.Clients.Persons;
using FirstCall.Application.Features.Products;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Application.Requests.Clients.Persons;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Country;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Application.Features.Countries.Queries.GetAll;

using FirstCall.Application.Requests.Products;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;


using System.Xml.Linq;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using Microsoft.Extensions.Logging;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Threading;
using DocumentFormat.OpenXml.Spreadsheet;
using FirstCall.Client.Pages.Products;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;
using FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder;
using FirstCall.Application.Requests.Deliveries.DeliveryOrderProducts;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class InsideDeliveryOrdersProducts
    {
        [Parameter] public int OrderId { get; set; } = 0;

        [Parameter] public List<AddEditDeliveryOrderProductCommand> Charges { get; set; } = new();
        [Parameter] public EventCallback<List<AddEditDeliveryOrderProductCommand>> ChargesChanged { get; set; }

        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");
        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IDeliveryOrderProductManager DeliveryOrderProductManager { get; set; }

        private List<GetAllPagedProductsResponse> _Products = new();



        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private AddEditDeliveryOrderProductCommand _LeaseCharge = new();


        private string _searchString = "";
        //private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private int _leaseId;
        private bool istransport;


        private ClaimsPrincipal _currentUser;
        private bool _canCreateLeaseCharges;
        private bool _canEditLeaseCharges;
        private bool _canDeleteLeaseCharges;
        private bool _canExportLeaseCharges;
        private bool _canSearchLeaseCharges;
        private bool _loaded;

        private decimal totalPrice = 0;


        public override async Task SetParametersAsync(ParameterView parameters)
        {
            _loaded = false;
            await base.SetParametersAsync(parameters);
            if (_leaseId != OrderId)
            {

                _leaseId = OrderId;
                await LoadData();
            }
            _loaded = true;
            StateHasChanged();

        }
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateLeaseCharges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Create)).Succeeded;
            _canEditLeaseCharges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Edit)).Succeeded;
            _canDeleteLeaseCharges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.Delete)).Succeeded;
            _canExportLeaseCharges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.View)).Succeeded;
            _canSearchLeaseCharges = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DeliveryOrders.View)).Succeeded;

            await LoadData();
             await LoadFees();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<IEnumerable<int>> SearchProducts(string value)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Products.Select(x => x.Id);

            return _Products.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        string ProductToString(int id)
        {
            var product = _Products.FirstOrDefault(b => b.Id == id);
            if (product is null)
                return string.Empty;

            return $"{product.NameAr} - {product.ProductCategory.NameAr} - {product.Kind.NameAr} - {product.Code}";
        }









        public async Task LoadFees()
        {
            var request = new GetAllPagedProductsRequest { PageNumber = 1, PageSize = 100000, Orderby = null, SearchString = null };
            var resopnse = await ProductManager.GetAllPagedAsync(request);
            if (resopnse.Succeeded)
            {
                _Products = resopnse.Data;
            }
        }

        private async Task LoadData()
        {
            _loaded = false;
            var request = new GetAllDeliveryOrderProductsRequest { deliveryorder = OrderId, PageNumber = 1, PageSize = int.MaxValue };
            var response = await DeliveryOrderProductManager.GetDeliveryOrderProductsAsync(request);
            if (response.Succeeded)
            {
                Charges = response.Data.Select(x => new AddEditDeliveryOrderProductCommand
                {
                    Id = x.Id,
                     DeliveryOrderId = x.DeliveryOrderId,
                      ProductId = x.ProductId,
                        Quantity  = x.Quantity,
                          TotalPrice = x.TotalPrice,
                           UnitPrice = x.UnitPrice,

                }).ToList();

                await ChargesChanged.InvokeAsync(Charges);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            _loaded = true;
        }

        private async Task Delete(AddEditDeliveryOrderProductCommand charge)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), deleteContent}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
        
                Charges.Remove(charge);
                await ChargesChanged.InvokeAsync(Charges);
            }
        }


        public async void InvokeModal()
        {
            Charges.Add(new AddEditDeliveryOrderProductCommand
            {

                 DeliveryOrderId = OrderId
            });
            await ChargesChanged.InvokeAsync(Charges);
        }

        private async Task<IEnumerable<int>> SearchFees(string value)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Products.Select(x => x.Id);

            return _Products.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private bool Search(AddEditDeliveryOrderProductCommand LeaseCharge)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;


            /**/
            return false;
        }


            public async Task SaveLeaseCharges(int leaseId)
        {
            foreach (var item in Charges)
            {
                if (OrderId == 0)
                    item.DeliveryOrderId = leaseId;
                var response = await DeliveryOrderProductManager.SaveAsync(item);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);

                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task ItemHasBeenCommitted(object e)
        {
            if (((AddEditDeliveryOrderProductCommand)e).Quantity  > 0) 
            {
                ((AddEditDeliveryOrderProductCommand)e).TotalPrice = ((AddEditDeliveryOrderProductCommand)e).UnitPrice * ((AddEditDeliveryOrderProductCommand)e).Quantity;
            }
            await ChargesChanged.InvokeAsync(Charges);

            //_snackBar.Add("Commited", Severity.Error);
            //await ChargesChanged.InvokeAsync(Charges);

        }


    }

}
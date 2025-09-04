using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Application.Features.Countries.Queries.GetAll;
using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Client.Infrastructure.Managers.Clients;
using FirstCall.Client.Infrastructure.Managers.Clients.Persons;
using FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Country;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Shared.Constants.Orders;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class AddEditDeliveryOrder
    {
        [Inject] private IDeliveryOrderManager DeliveryOrderManager { get; set; }


        private InsideDeliveryOrdersProducts _leaseCharges;

        [Inject] private IPrincedomManager PrincedomManager { get; set; }
        [Inject] private ICountryManager CountryManager { get; set; }
 


        [Inject] private IPersonManager PersonManager { get; set; }

        private bool _isProcessing = false;
        [Parameter] public int OrderId { get; set; } = 0;


        public AddEditDeliveryOrderCommand AddEditDeliveryOrderModel { get; set; } = new();

        private FluentValidationValidator _fluentValidationValidator;
        //private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });


        private List<GetAllPersonsResponse> _persons = new();
 


        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await Task.WhenAll(


                           LoadPersonsAsync(),

                           LoadOrderDetails()
                


                           );
        }

   






        private async Task LoadPersonsAsync()
        {
            var data = await PersonManager.GetAllAsync();
            if (data.Succeeded)
            {
                _persons = data.Data;
            }
        }



        private async Task LoadOrderDetails()
        {
            if (OrderId != 0)
            {
                var data = await DeliveryOrderManager.GetByIdAsync(OrderId);
                if (data.Succeeded)
                {
                    var order = data.Data;
                    AddEditDeliveryOrderModel = new AddEditDeliveryOrderCommand
                    {

                        ClientId = order.ClientId,
                        Id = order.Id,
                        OrderNumber = order.OrderNumber,
                        TotalPrice = order.TotalPrice,
                        Status = order.Status,
                       OrderDate = order.OrderDate
                    };

              
   

                }
            }
            else
            {
                AddEditDeliveryOrderModel.Id = 0;
            }

        }

        private async Task Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }


        private async Task SaveAsync()
        {


            _isProcessing = true;
       
            if (string.IsNullOrEmpty(AddEditDeliveryOrderModel.Status))
                AddEditDeliveryOrderModel.Status = OrderStatusEnum.Pending.ToString();

            AddEditDeliveryOrderModel.TotalPrice = _leaseCharges.Charges.Sum(x => x.TotalPrice);

            var response = await DeliveryOrderManager.SaveAsync(AddEditDeliveryOrderModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                AddEditDeliveryOrderModel.Id = response.Data;
                OrderId = response.Data;
                if (OrderId != 0)
                {
                    await Task.WhenAll(_leaseCharges.SaveLeaseCharges(OrderId));
                }
                await Cancel();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            _isProcessing = false;
        }







        private async Task<IEnumerable<int>> SearchPersons(string value)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _persons.Select(x => x.ClientId);

            return _persons.Where(x => x.FullName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.ClientId);

        }

    }
}

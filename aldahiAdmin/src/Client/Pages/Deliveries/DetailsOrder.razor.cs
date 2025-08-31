using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Application.Models.Chat;
using FirstCall.Client;
using FirstCall.Client.Infrastructure.Managers.Clients.Persons;
using FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder;
using FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom;
using FirstCall.Core.Entities;
using FirstCall.Shared.Constants.Clients;
using FirstCall.Shared.Constants.Orders;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;

namespace FirstCall.Client.Pages.Deliveries
{
    public partial class DetailsOrder
    {
        [Inject] private IDeliveryOrderManager DeliveryOrderManager { get; set; }



        [Inject] private IPersonManager PersonManager { get; set; }


        [Parameter] public int OrderId { get; set; } = 0;
        private List<GetAllPrincedomsResponse> _princedoms = new();


        public AddEditDeliveryOrderCommand AddEditDeliveryOrderModel { get; set; } = new();

        private FluentValidationValidator _fluentValidationValidator;
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
                        OrderDate  =order.OrderDate,
                    };
      
                }
            }
            else
            {
                AddEditDeliveryOrderModel.Id = 0;
            }

        }

        private void ChangeStatus()
        {


        }
        private async Task Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }



        private async Task SaveAsync()
        {
        
           
            AddEditDeliveryOrderModel.Status = OrderStatusEnum.Pending.ToString();

            var response = await DeliveryOrderManager.SaveAsync(AddEditDeliveryOrderModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                AddEditDeliveryOrderModel.Id = response.Data;
                OrderId = response.Data;

                await LoadOrderDetails();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
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

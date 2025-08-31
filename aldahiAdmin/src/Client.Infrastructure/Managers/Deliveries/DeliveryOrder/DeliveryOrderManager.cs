using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Application.Features.DeliveryOrders.Queries.GetById;
using FirstCall.Application.Requests.Deliveries.DeliveryOrders;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder
{
    public class DeliveryOrderManager:IDeliveryOrderManager
    {
        private readonly HttpClient _httpClient;
        public DeliveryOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DeliveryOrdersEndpoints.Export
                : Routes.DeliveryOrdersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DeliveryOrdersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDeliveryOrdersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrdersEndpoints.GetAll);
            return await response.ToResult<List<GetAllDeliveryOrdersResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDeliveryOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DeliveryOrdersEndpoints.Save, request);
            return await response.ToResult<int>();
        }



        public async Task<IResult<GetDeliveryOrderByIdResponse>> GetByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrdersEndpoints.GetById(orderId));
            return await response.ToResult<GetDeliveryOrderByIdResponse>();
        }

        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllPagedAsync(GetAllPagedDeliveryOrdersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrdersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllDeliveryOrdersResponse>();
        }

        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllPagedByClientAsync(GetAllPagedDeliveryOrdersRequest request, int clientId)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrdersEndpoints.GetAllPagedByClient(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, clientId));
            return await response.ToPaginatedResult<GetAllDeliveryOrdersResponse>();
        }





        public async Task<PaginatedResult<GetAllDeliveryOrdersResponse>> GetAllByStatusAsync(GetAllPagedDeliveryOrdersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrdersEndpoints.GetAllByStatus(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, request.Status));
            return await response.ToPaginatedResult<GetAllDeliveryOrdersResponse>();
        }

      

        public async Task<IResult<int>> RefuseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DeliveryOrdersEndpoints.Refuse}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> AccceptOrderRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DeliveryOrdersEndpoints.AccceptOrderRequest}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> OrderCloseRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DeliveryOrdersEndpoints.OrderCloseRequest}/{id}");
            return await response.ToResult<int>();
        }

   
    }
}

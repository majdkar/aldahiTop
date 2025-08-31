using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;
using FirstCall.Application.Features.OrderProducts.Queries.GetById;
using FirstCall.Application.Requests.Deliveries.DeliveryOrderProducts;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrder;
using FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder;

namespace FirstCall.Client.Infrastructure.Managers.Deliveries.DeliveryOrderProduct
{
    public class DeliveryOrderProductManager:IDeliveryOrderProductManager
    {
        private readonly HttpClient _httpClient;
        public DeliveryOrderProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<GetAllDeliveryOrderProductsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrderProductsEndpoints.GetAll);
            return await response.ToResult<List<GetAllDeliveryOrderProductsResponse>>();
        }



        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DeliveryOrderProductsEndpoints.Export
                : Routes.DeliveryOrderProductsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DeliveryOrderProductsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }
        public async Task<PaginatedResult<GetAllDeliveryOrderProductsResponse>> GetDeliveryOrderProductsAsync(GetAllDeliveryOrderProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrderProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, request.deliveryorder));
            return await response.ToPaginatedResult<GetAllDeliveryOrderProductsResponse>();
        }
        public async Task<IResult<int>> SaveAsync(AddEditDeliveryOrderProductCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DeliveryOrderProductsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> GetDeliveryOrderProductAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.DeliveryOrderProductsEndpoints.GetDeliveryOrderProductById(id));
            return await response.ToResult<string>();
        }
    }
}

using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Warehousess;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Warehouses
{
    public class WarehousesManager:IWarehousesManager
    {
        private readonly HttpClient _httpClient;

        public WarehousesManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedResult<GetAllWarehousessResponse>> GetAllPagedAsync(GetAllPagedWarehousesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.WarehousessEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllWarehousessResponse>();
        }
        public async Task<IResult<List<GetAllWarehousessResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.WarehousessEndpoints.GetAll);
            return await response.ToResult<List<GetAllWarehousessResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.WarehousessEndpoints.Export
                : Routes.WarehousessEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.WarehousessEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditWarehousesCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.WarehousessEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}

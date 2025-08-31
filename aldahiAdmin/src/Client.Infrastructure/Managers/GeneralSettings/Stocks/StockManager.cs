using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Stocks.Commands.AddEdit;
using FirstCall.Application.Features.Stocks.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Stocks;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Stock
{
    public class StockManager:IStockManager
    {
        private readonly HttpClient _httpClient;

        public StockManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedResult<GetAllStocksResponse>> GetAllPagedAsync(GetAllPagedStockRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.StocksEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllStocksResponse>();
        }
        public async Task<IResult<List<GetAllStocksResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.StocksEndpoints.GetAll);
            return await response.ToResult<List<GetAllStocksResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.StocksEndpoints.Export
                : Routes.StocksEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.StocksEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditStockCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.StocksEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}

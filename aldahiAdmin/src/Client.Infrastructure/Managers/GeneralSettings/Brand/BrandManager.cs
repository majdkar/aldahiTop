using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Commands.AddEdit;
using FirstCall.Application.Requests.Brand;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Brand
{
    public class BrandManager : IBrandManager
    {
        private readonly HttpClient _httpClient;

        public BrandManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.BrandsEndpoints.Export
                : Routes.BrandsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }
        public async Task<IResult<string>> GetbrandImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.BrandsEndpoints.GetbrandImage(id));
            return await response.ToResult<string>();
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.BrandsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }


        public async Task<PaginatedResult<GetAllBrandsResponse>> GetAllPagedAsync(GetAllPagedBrandRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.BrandsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllBrandsResponse>();
        }


        public async Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BrandsEndpoints.GetAll);
            return await response.ToResult<List<GetAllBrandsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBrandCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BrandsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
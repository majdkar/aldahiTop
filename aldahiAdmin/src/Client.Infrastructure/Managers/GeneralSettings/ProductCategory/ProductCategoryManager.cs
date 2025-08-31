using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged;

using FirstCall.Application.Requests.ProductCategories;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.ProductCategory
{
    public class ProductCategoryManager : IProductCategoryManager
    {
        private readonly HttpClient _httpClient;

        public ProductCategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductCategoriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ProductCategoriesEndpoints.Export
                : Routes.ProductCategoriesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetProductCategoryImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetProductCategoryImage(id));
            return await response.ToResult<string>();
        }
        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedProductCategoriesRequest request, int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllPagedSons(request.Type,categoryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductCategoriesResponse>();
        }
        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetPagedByTypeAsync(GetAllPagedProductCategoriesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllPagedByType(request.Type, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductCategoriesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductCategoriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllByTypeAsync(string type)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllByType(type));
            return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        }
        public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        }
        //public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllParentCategoriesByTypeAsync(string type)
        //{
        //    var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllParentCategoriesByType(type));
        //    return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        //}

        public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllByParentCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllByParentCategory(categoryId));
            return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        }
        
    }
}

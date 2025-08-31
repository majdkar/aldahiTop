
using FirstCall.Application.Features.ProductComponents.Commands.AddEdit;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Requests.Products;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.ProductComponents
{
    public class ProductComponentManager : IProductComponentManager
    {
        private readonly HttpClient _httpClient;
        public ProductComponentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedAsync(GetAllPagedProductComponentsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductComponentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductComponentsResponse>();
        }
           public async Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedProductIdAsync(GetAllPagedProductComponentsRequest request,int ProductId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductComponentsEndpoints.GetAllPagedByProductId(request.PageNumber, request.PageSize, request.SearchString, request.Orderby,ProductId));
            return await response.ToPaginatedResult<GetAllPagedProductComponentsResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedSearchProductComponentAsync(GetAllPagedProductComponentsRequest request, string ProductComponentname, decimal fromprice, decimal toprice)
        {
            var response = await _httpClient.GetAsync(Routes.ProductComponentsEndpoints.GetAllPagedSearchProductComponent(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, ProductComponentname,  fromprice,  toprice));
            return await response.ToPaginatedResult<GetAllPagedProductComponentsResponse>();
        }

 




        public async Task<IResult<GetProductComponentByIdResponse>> GetByIdAsync(int ProductComponentId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductComponentsEndpoints.GetProductComponentById(ProductComponentId));
            return await response.ToResult<GetProductComponentByIdResponse>();
        }

        public async Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductComponentCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductComponentsEndpoints.SaveForCompanyProfile, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductComponentsEndpoints.DeleteProductComponent}/{id}");
            return await response.ToResult<int>();
        }

    
        public async Task<IResult<List<GetAllProductComponentsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductComponentsEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductComponentsResponse>>();
        }



    }
}

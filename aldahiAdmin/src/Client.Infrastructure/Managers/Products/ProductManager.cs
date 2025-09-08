 
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Requests.Products;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.Products
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;
        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedAsync(GetAllPagedProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby,request.ProductType));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedSearchProductAsync(GetAllPagedProductsRequest request, string productname, decimal fromprice, decimal toprice,string ProductType)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedSearchProduct(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, productname,  fromprice,  toprice,ProductType));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }



        public async Task<IResult<string>> GetAllByForDownloadReportAsync(int seasonId, int kindId, int groupId, int warehousesId, int productCategoryId, string code, int fromqty, int toqty, string ProductType)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllByForDownloadReportAsync(seasonId,kindId,groupId,warehousesId,productCategoryId,code,fromqty,toqty,ProductType ));
            return await response.ToResult<string>();
        }


        public async Task<IResult<GetProductByIdResponse>> GetByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetProductById(productId));
            return await response.ToResult<GetProductByIdResponse>();
        }

        public async Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.SaveForCompanyProfile, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductsEndpoints.DeleteProduct}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.ProductsEndpoints.ExportFilteredByCompany(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllProductsResponse>>> GetAllAsync(string ProductType)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAll(ProductType));
            return await response.ToResult<List<GetAllProductsResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedSearchAdvancedProductAsync( int seasonId, int kindId, int groupId, int warehousesId, int productCategoryId, string code, int fromqty, int toqty, string ProductType)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedSearchAdvancedProduct( seasonId,kindId,groupId,warehousesId,productCategoryId,code,fromqty,toqty, ProductType));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }
    }
}

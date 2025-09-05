using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Requests.Products;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.Products
{
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedAsync(GetAllPagedProductsRequest request);


        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedSearchProductAsync(GetAllPagedProductsRequest request,string productname, decimal fromprice, decimal toprice,string ProductType);


   

        Task<IResult<GetProductByIdResponse>> GetByIdAsync(int productId,string ProductType);


        Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<List<GetAllProductsResponse>>> GetAllAsync(string ProductType);
    }
}

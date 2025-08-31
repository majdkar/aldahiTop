using FirstCall.Application.Features.ProductComponents.Commands.AddEdit;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetAllPaged;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Application.Requests.Products;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.ProductComponents
{
    public interface IProductComponentManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedAsync(GetAllPagedProductComponentsRequest request);
        Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedProductIdAsync(GetAllPagedProductComponentsRequest request,int ProductId);


        Task<PaginatedResult<GetAllPagedProductComponentsResponse>> GetAllPagedSearchProductComponentAsync(GetAllPagedProductComponentsRequest request,string ProductComponentname, decimal fromprice, decimal toprice);

        Task<IResult<GetProductComponentByIdResponse>> GetByIdAsync(int ProductComponentId);


        Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductComponentCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<List<GetAllProductComponentsResponse>>> GetAllAsync();
    }
}

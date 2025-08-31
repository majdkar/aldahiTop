using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Features.Brands.Commands.AddEdit;
using FirstCall.Application.Requests.Brand;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Brand
{
    public interface IBrandManager : IManager
    {


        Task<PaginatedResult<GetAllBrandsResponse>> GetAllPagedAsync(GetAllPagedBrandRequest request);


        Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditBrandCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<string>> GetbrandImageAsync(int id);
    }
}
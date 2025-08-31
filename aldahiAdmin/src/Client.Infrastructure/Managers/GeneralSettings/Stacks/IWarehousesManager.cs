
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Warehousess.Commands.AddEdit;
using FirstCall.Application.Features.Warehousess.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Warehousess;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Warehouses
{
    public interface IWarehousesManager:IManager
    {
        Task<PaginatedResult<GetAllWarehousessResponse>> GetAllPagedAsync(GetAllPagedWarehousesRequest request);
        Task<IResult<List<GetAllWarehousessResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditWarehousesCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}

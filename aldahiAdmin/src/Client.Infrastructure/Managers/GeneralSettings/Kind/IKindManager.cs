
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Kinds;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Kind
{
    public interface IKindManager:IManager
    {
        Task<PaginatedResult<GetAllKindsResponse>> GetAllPagedAsync(GetAllPagedKindRequest request);
        Task<IResult<List<GetAllKindsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditKindCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}

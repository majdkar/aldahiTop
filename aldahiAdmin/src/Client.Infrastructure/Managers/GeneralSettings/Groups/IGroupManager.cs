
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Groups.Commands.AddEdit;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Groups;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Group
{
    public interface IGroupManager:IManager
    {
        Task<PaginatedResult<GetAllGroupsResponse>> GetAllPagedAsync(GetAllPagedGroupRequest request);
        Task<IResult<List<GetAllGroupsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditGroupCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}

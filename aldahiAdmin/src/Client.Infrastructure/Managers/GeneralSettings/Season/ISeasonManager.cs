
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Seasons;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Season
{
    public interface ISeasonManager:IManager
    {
        Task<PaginatedResult<GetAllSeasonsResponse>> GetAllPagedAsync(GetAllPagedSeasonRequest request);
        Task<IResult<List<GetAllSeasonsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditSeasonCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}

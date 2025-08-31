using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Features.Nations.Commands.AddEdit;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Nation
{
    public interface INationManager : IManager
    {
        Task<IResult<List<GetAllNationsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditNationCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
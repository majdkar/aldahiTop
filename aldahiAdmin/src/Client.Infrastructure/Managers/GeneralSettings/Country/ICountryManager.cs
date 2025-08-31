using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Features.Countries.Commands.AddEdit;
using FirstCall.Application.Features.Countries.Queries.GetAll;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Country
{
    public interface ICountryManager : IManager
    {
        Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCountryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
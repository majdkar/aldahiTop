using System;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom
{
    public interface IPrincedomManager : IManager
    {
        Task<IResult<List<GetAllPrincedomsResponse>>> GetAllAsync();
        Task<IResult<List<GetAllPrincedomsResponse>>> GetByIdAsync(int id);


        Task<IResult<int>> SaveAsync(AddEditPrincedomCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
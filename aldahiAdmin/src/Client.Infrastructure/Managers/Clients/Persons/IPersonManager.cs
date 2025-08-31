using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;
using FirstCall.Application.Requests.Clients.Persons;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Client.Infrastructure.Managers.Clients.Persons
{
    public interface IPersonManager:IManager
    {
        Task<IResult<List<GetAllPersonsResponse>>> GetAllAsync();
        Task<IResult<GetAllPersonsResponse>> GetByClientIdAsync(int clientId);


        Task<PaginatedResult<GetAllPersonsResponse>> GetPersonsAsync(GetAllPagedPersonsRequest request);

        Task<IResult<GetAllPersonsResponse>> GetByIdAsync(int personId);

        Task<IResult<int>> SaveAsync(AddEditPersonCommand request);


        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}

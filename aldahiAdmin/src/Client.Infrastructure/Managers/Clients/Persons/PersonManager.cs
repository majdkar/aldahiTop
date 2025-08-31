using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;

using FirstCall.Application.Requests.Clients.Persons;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Client.Infrastructure.Managers.Clients.Persons
{
    public class PersonManager : IPersonManager
    {
        private readonly HttpClient _httpClient;

        public PersonManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllPersonsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetAll);
            return await response.ToResult<List<GetAllPersonsResponse>>();
        }

        public async Task<IResult<GetAllPersonsResponse>> GetByIdAsync(int PersonId)
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetById(PersonId));
            return await response.ToResult<GetAllPersonsResponse>();
        }


        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PersonsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }


        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PersonsEndpoints.Export
                : Routes.PersonsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPersonCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllPersonsResponse>> GetPersonsAsync(GetAllPagedPersonsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetAllPaged(request.PersonName, request.Email, request.PhoneNumber,request.CountryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPersonsResponse>();
        }

        public async Task<IResult<GetAllPersonsResponse>> GetByClientIdAsync(int clientId)
        {
            var response = await _httpClient.GetAsync(Routes.PersonsEndpoints.GetByClientId(clientId));
            return await response.ToResult<GetAllPersonsResponse>();
        }
    }
}

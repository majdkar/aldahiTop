using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FirstCall.Application.Features.Nations.Commands.AddEdit;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Nation
{
    public class NationManager : INationManager
    {
        private readonly HttpClient _httpClient;

        public NationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.NationsEndpoints.Export
                : Routes.NationsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.NationsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllNationsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.NationsEndpoints.GetAll);
            return await response.ToResult<List<GetAllNationsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditNationCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.NationsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
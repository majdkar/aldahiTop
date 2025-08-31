using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FirstCall.Application.Features.Countries.Commands.AddEdit;
using FirstCall.Application.Features.Countries.Queries.GetAll;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Country
{
    public class CountryManager : ICountryManager
    {
        private readonly HttpClient _httpClient;

        public CountryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CountriesEndpoints.Export
                : Routes.CountriesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CountriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCountriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CountriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCountriesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCountryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CountriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

      
    }
}
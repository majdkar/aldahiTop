using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Seasons;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Season
{
    public class SeasonManager:ISeasonManager
    {
        private readonly HttpClient _httpClient;

        public SeasonManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedResult<GetAllSeasonsResponse>> GetAllPagedAsync(GetAllPagedSeasonRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.SeasonsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllSeasonsResponse>();
        }
        public async Task<IResult<List<GetAllSeasonsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.SeasonsEndpoints.GetAll);
            return await response.ToResult<List<GetAllSeasonsResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.SeasonsEndpoints.Export
                : Routes.SeasonsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.SeasonsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditSeasonCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SeasonsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}

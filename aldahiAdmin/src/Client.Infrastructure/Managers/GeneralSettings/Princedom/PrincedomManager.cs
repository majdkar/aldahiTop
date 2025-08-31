using System;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Princedom
{
    public class PrincedomManager : IPrincedomManager
    {
        private readonly HttpClient _httpClient;

        public PrincedomManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PrincedomsEndpoints.Export
                : Routes.PrincedomsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PrincedomsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllPrincedomsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PrincedomsEndpoints.GetAll);
            return await response.ToResult<List<GetAllPrincedomsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPrincedomCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PrincedomsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<List<GetAllPrincedomsResponse>>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{Routes.PrincedomsEndpoints.GetById}/{id}");
            return await response.ToResult<List<GetAllPrincedomsResponse>>();
        }
    }
}
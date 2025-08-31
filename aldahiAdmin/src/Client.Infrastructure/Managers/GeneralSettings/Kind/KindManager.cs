using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Kinds;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Kind
{
    public class KindManager:IKindManager
    {
        private readonly HttpClient _httpClient;

        public KindManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedResult<GetAllKindsResponse>> GetAllPagedAsync(GetAllPagedKindRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.KindsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllKindsResponse>();
        }
        public async Task<IResult<List<GetAllKindsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.KindsEndpoints.GetAll);
            return await response.ToResult<List<GetAllKindsResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.KindsEndpoints.Export
                : Routes.KindsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.KindsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditKindCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.KindsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}

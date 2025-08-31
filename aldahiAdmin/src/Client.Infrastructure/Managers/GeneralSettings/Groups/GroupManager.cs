using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Groups.Commands.AddEdit;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Application.Requests.Brand;
using FirstCall.Application.Requests.Groups;
using FirstCall.Client.Infrastructure.Extensions;
using FirstCall.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.GeneralSettings.Group
{
    public class GroupManager:IGroupManager
    {
        private readonly HttpClient _httpClient;

        public GroupManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PaginatedResult<GetAllGroupsResponse>> GetAllPagedAsync(GetAllPagedGroupRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.GroupsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllGroupsResponse>();
        }
        public async Task<IResult<List<GetAllGroupsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.GroupsEndpoints.GetAll);
            return await response.ToResult<List<GetAllGroupsResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.GroupsEndpoints.Export
                : Routes.GroupsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.GroupsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditGroupCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.GroupsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}

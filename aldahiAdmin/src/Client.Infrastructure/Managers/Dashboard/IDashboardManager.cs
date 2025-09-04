using FirstCall.Shared.Wrapper;
using System.Threading.Tasks;
using FirstCall.Application.Features.Dashboards.Queries.GetData;
using System.Collections.Generic;

namespace FirstCall.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
        Task<IResult<List<DashboardDataProductResponse>>> GetDataProductAsync();
    }
}
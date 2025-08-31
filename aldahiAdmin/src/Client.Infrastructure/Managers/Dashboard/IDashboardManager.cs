using FirstCall.Shared.Wrapper;
using System.Threading.Tasks;
using FirstCall.Application.Features.Dashboards.Queries.GetData;

namespace FirstCall.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}
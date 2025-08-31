using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Interfaces.Common;
using FirstCall.Application.Requests.Identity;
using FirstCall.Application.Responses.Identity;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}
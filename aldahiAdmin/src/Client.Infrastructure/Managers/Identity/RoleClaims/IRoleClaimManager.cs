using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Requests.Identity;
using FirstCall.Application.Responses.Identity;
using FirstCall.Shared.Wrapper;

namespace FirstCall.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}
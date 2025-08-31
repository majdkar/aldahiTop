using FirstCall.Application.Requests.Identity;
using FirstCall.Shared.Wrapper;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Managers.Identity.Account
{
    public interface IAccountManager : IManager
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model);
        Task<IResult> ChangePasswordByAdminAsync(ChangePasswordByAdminRequest model);
        Task<IResult> ResetPasswordAndEmailAsync(ResetPasswordAndEmailRequest model);
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model);
        Task<IResult> UpdateProfileByAdminAsync(UpdateProfileByAdminRequest model);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}
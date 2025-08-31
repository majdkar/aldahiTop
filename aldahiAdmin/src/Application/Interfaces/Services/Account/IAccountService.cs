using FirstCall.Application.Interfaces.Common;
using FirstCall.Application.Requests.Identity;
using FirstCall.Shared.Wrapper;
using System.Threading.Tasks;

namespace FirstCall.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);
        Task<IResult> UpdateProfileByAdminAsync(UpdateProfileByAdminRequest model);
        Task<IResult> ResetPasswordAndEmailAsync(ResetPasswordAndEmailRequest model, string userId, bool isAdmin);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);
        Task<IResult> ChangePasswordByAdminAsync(ChangePasswordByAdminRequest model);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);

    }
}
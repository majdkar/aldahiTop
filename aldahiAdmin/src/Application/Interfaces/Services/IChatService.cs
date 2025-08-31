using FirstCall.Application.Responses.Identity;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Application.Models.Chat;
using FirstCall.Domain.Interfaces.Chat;

namespace FirstCall.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}
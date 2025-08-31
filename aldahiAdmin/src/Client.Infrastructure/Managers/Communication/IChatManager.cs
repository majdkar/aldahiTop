using FirstCall.Application.Models.Chat;
using FirstCall.Application.Responses.Identity;
using FirstCall.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Domain.Interfaces.Chat;

namespace FirstCall.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
    }
}
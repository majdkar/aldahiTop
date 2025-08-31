using AutoMapper;
using FirstCall.Application.Models.Chat;
using FirstCall.Domain.Interfaces.Chat;
using FirstCall.Infrastructure.Models.Identity;

namespace FirstCall.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<BlazorHeroUser>>().ReverseMap();
        }
    }
}
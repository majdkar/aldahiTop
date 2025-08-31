using FirstCall.Application.Interfaces.Common;

namespace FirstCall.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}
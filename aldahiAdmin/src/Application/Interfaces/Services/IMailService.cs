using FirstCall.Application.Requests.Mail;
using System.Threading.Tasks;

namespace FirstCall.Application.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}
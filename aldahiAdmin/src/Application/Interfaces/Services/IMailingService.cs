using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Application.Requests.Mail;

namespace FirstCall.Application.Interfaces.Services
{
    public interface IMailingService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}

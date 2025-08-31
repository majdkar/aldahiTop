using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests.Mail;
using FirstCall.Shared.Wrapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Security.Authentication;
using System.Runtime;
using MimeKit.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FirstCall.Application.Features.ContactUs.Commands.AddEdit
{
    public partial class SendContactUsCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }

    internal class SendContactUsCommandHandler : IRequestHandler<SendContactUsCommand, Result<int>>
    {
        private readonly IMapper _mapper;

        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly MailSettings _mailSettings;

        public SendContactUsCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IOptions<MailSettings> mailSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _mailSettings = mailSettings.Value;
        }

      
        public async Task<Result<int>> Handle(SendContactUsCommand command, CancellationToken cancellationToken)
        {

             await SendEmail(command);   

            return await Result<int>.SuccessAsync("Send Email Ok");

        }



        public async Task SendEmail(SendContactUsCommand request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.UserName));
            email.To.Add(MailboxAddress.Parse(_mailSettings.To));
            email.Subject = "Contact US Urgent Online";

            var builder = new BodyBuilder();

            builder.HtmlBody = "<table style='border: 1px solid black'>" +
            "<th colspan=2 style='border: 1px solid black'>Contact Us</th>" +
                "<tr><td style='border: 1px solid black;text-align: center'><b>Name</b></td><td style='border: 1px solid black;text-align: center'> " + request.Name + "</td></tr>" +
                "<tr><td style='border: 1px solid black;text-align: center'><b>Phone</b></td><td style='border: 1px solid black;text-align: center'> " + request.Phone + "</td></tr>" +
                 "<tr><td style='border: 1px solid black;text-align: center'><b>Email</b></td><td style='border: 1px solid black;text-align: center'> " + request.Email + "</td></tr>" +
                  "<tr><td style='border: 1px solid black;text-align: center'><b>Message</b></td><td style='border: 1px solid black;text-align: center'> " + request.Message + "</td></tr>" +
                 "</table>" + "<br>" + "<br>" +
                "<table style='border: 1px solid black'>" +
                "<th colspan=2 style='border: 1px solid black'>تواصل معنا</th>" +
                "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + request.Name + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الاسم</b></td></tr>" +
                "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + request.Phone + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الهاتف</b></td></tr>" +
                "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + request.Email + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الإيميل</b></td></tr>" +
                "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + request.Message + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الرسالة</b></td></tr>" +
                 "</table>";

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, 587, SecureSocketOptions.None);
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }



        //public async Task SendEmailAsync(SendContactUsCommand command, CancellationToken ct)
        //{
        //    var email = new MimeMessage();

        //    email.Sender = MailboxAddress.Parse(_mailSettings.UserName);
        //    email.From.Add(MailboxAddress.Parse(_mailSettings.UserName));
        //    email.To.Add(MailboxAddress.Parse(command.Email));
        //    email.Subject = "Contact US Urgent Online";

        //    var builder = new BodyBuilder();

        //    builder.HtmlBody = "<table style='border: 1px solid black'>" +
        //        "<th colspan=2 style='border: 1px solid black'>Contact Us</th>" +
        //        "<tr><td style='border: 1px solid black;text-align: center'><b>Name</b></td><td style='border: 1px solid black;text-align: center'> " + command.Name + "</td></tr>" +
        //        "<tr><td style='border: 1px solid black;text-align: center'><b>Phone</b></td><td style='border: 1px solid black;text-align: center'> " + command.Phone + "</td></tr>" +
        //         "<tr><td style='border: 1px solid black;text-align: center'><b>Email</b></td><td style='border: 1px solid black;text-align: center'> " + command.Email + "</td></tr>" +
        //          "<tr><td style='border: 1px solid black;text-align: center'><b>Message</b></td><td style='border: 1px solid black;text-align: center'> " + command.Message + "</td></tr>" +
        //         "</table>"+ "<br>" + "<br>" +
        //        "<table style='border: 1px solid black'>" +
        //        "<th colspan=2 style='border: 1px solid black'>تواصل معنا</th>" +
        //        "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + command.Name + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الأسم</b></td></tr>" +
        //        "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + command.Phone + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الهاتف</b></td></tr>" +
        //        "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + command.Email + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الإيميل</b></td></tr>" +
        //        "<tr><td style='border: 1px solid black;text-align: center; direction: rtl'>" + command.Message + "</td><td style='border: 1px solid black;text-align: center; direction: rtl'><b>الرسالة</b></td></tr>" +
        //         "</table>";
        //    email.Body = builder.ToMessageBody();
        //    //using var smtp = new SmtpClient();
        //    //smtp.Connect(_mailSettings.Host, _mailSettings.Port, Convert.ToBoolean(_mailSettings.ssl));
        //    //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    //await smtp.SendAsync(email);
        //    //smtp.Disconnect(true);
        //    //using var smtp = new SmtpClient();
        //    //smtp.CheckCertificateRevocation = false;
        //    //smtp.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
        //    //if (_mailSettings.UseSSL)
        //    //{
        //    //    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //    //    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto, ct);
        //    //}
        //    //else if (_mailSettings.UseStartTls)
        //    //{
        //    //    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto, ct);
        //    //}
        //    //else
        //    //{
        //    //    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto, ct);
        //    //}

        //    //await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, ct);
        //    //await smtp.SendAsync(email, ct);
        //    //await smtp.DisconnectAsync(true, ct);
        //    using var smtp = new SmtpClient();

        //    if (_mailSettings.UseSSL)
        //    {
        //        smtp.ServerCertificateValidationCallback = (l, j, c, m) => true;
        //        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port465, SecureSocketOptions.SslOnConnect, ct);
        //    }
        //    else if (_mailSettings.UseStartTls)
        //    {
        //        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port465, SecureSocketOptions.StartTls, ct);
        //    }
        //    else
        //    {
        //        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port587, SecureSocketOptions.None, ct);
        //    }

        //    await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, ct);
        //    await smtp.SendAsync(email, ct);
        //    await smtp.DisconnectAsync(true, ct);
        //}






    }
}

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FirstCall.Application.Requests.Mail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public string From { get; set; }

        public List<IFormFile> Attachments { get; set; }
    }
}
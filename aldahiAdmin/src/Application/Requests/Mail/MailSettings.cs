using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Mail
{
    public class MailSettings
    {
        //public string Mail { get; set; }
        //public string DisplayName { get; set; }
        //public string Password { get; set; }
        //public string Host { get; set; }
        //public int Port { get; set; }
        //public string RecipientEmail { get; set; }

        //public string ssl { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Host { get; set; }
        public int Port465 { get; set; }
        public int Port587 { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
    }

    public class OpenAIConfig
    {
        public string ApiKey { get; set; }
    }

}

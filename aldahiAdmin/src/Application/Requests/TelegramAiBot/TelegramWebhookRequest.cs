using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FirstCall.Application.Requests.TelegramAiBot
{
    public class TelegramWebhookRequest
    {
        public Update Update { get; set; }

    }
}

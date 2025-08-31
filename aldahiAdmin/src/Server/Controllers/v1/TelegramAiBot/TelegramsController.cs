using FirstCall.Application.Features.ContactUs.Commands.AddEdit;

using FirstCall.Application.Models.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FirstCall.Server.Controllers.v1.Subscribers;

[AllowAnonymous]
[Route("api/webhook")]
public class TelegramsController : BaseApiController<TelegramsController>
{

    private readonly ITelegramBotClient _botClient;

    public TelegramsController(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }


    [HttpPost("AddTelegram")]
    public async Task<IActionResult> Post(long chatId, string message)

        => Ok(await _mediator.Send(new SendTelegramAiBotCommand {  chatId = chatId,  message = message }));

    

    [HttpPost("AddTelegramAI")]
    public async Task<IActionResult> PostTelegramAI(long chatId, string naturalLanguageRequest ,string tableSchema)

        => Ok(await _mediator.Send(new SendTelegramAiBotByAICommand {  chatId = chatId,  naturalLanguageRequest = naturalLanguageRequest, tableSchema = tableSchema }));




    [HttpPost("getmessage")]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        // Only process text messages
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
        
        // Optional: reply to user
           await _mediator.Send(new SendTelegramAiBotCommand { chatId = chatId, message = messageText });

        }

        return Ok();
    }
}
//1955937367


//curl -X POST "https://api.telegram.org/bot7054212198:AAEfZRk1oVEqI5DeMLmFyL5q1dD-MNbkn80/setWebhook" \
//     -d "url=https://localhost:44390/api/webhook"

//    curl -X POST "https://api.telegram.org/bot7054212198:AAEfZRk1oVEqI5DeMLmFyL5q1dD-MNbkn80/setWebhook?url=https://localhost:44390/api/webhook"

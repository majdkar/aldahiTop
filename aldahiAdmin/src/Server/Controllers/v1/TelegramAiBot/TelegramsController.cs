using FirstCall.Application.Features.ContactUs.Commands.AddEdit;

using FirstCall.Application.Models.Chat;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Text.Json;
using FirstCall.Application.Requests.Mail;
using Microsoft.Extensions.Options;
using FirstCall.Server.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace FirstCall.Server.Controllers.v1.Subscribers;

[AllowAnonymous]
[Route("api/webhook")]
public class TelegramsController : BaseApiController<TelegramsController>
{

    private readonly ITelegramBotClient _botClient;
    private readonly HttpClient _httpClient;
    private readonly OpenAIConfig _openAIConfig;
    private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

    public TelegramsController(ITelegramBotClient botClient, HttpClient httpClient, IOptions<OpenAIConfig> openAIConfig)
    {
        _botClient = botClient;
        _httpClient = httpClient;
        _openAIConfig = openAIConfig.Value;
    }


    [HttpPost("AddTelegram")]
    public async Task<IActionResult> Post(long chatId, string message)

        => Ok(await _mediator.Send(new SendTelegramAiBotCommand {  chatId = chatId,  message = message }));



    //[HttpPost("AddTelegramAI")]
    //public async Task<IActionResult> PostTelegramAI([FromBody] TelegramUpdate update)
    //{

    //    if (update?.message == null || string.IsNullOrWhiteSpace(update.message.text))
    //        return Ok(); // تجاهل إذا ما في رسالة نصية

    //    var chatId = update.message.chat.id;
    //    var userMessage = update.message.text;

    //    try
    //    {
    //        await _mediator.Send(new SendTelegramAiBotByAICommand
    //        {
    //            naturalLanguageRequest = userMessage,
    //            chatId = chatId
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        // يمكنك تسجيل الخطأ هنا أيضاً
    //        await _botClient.SendMessage(
    //            chatId: chatId,
    //            text: "لا يوجد فكرة حاول مجدداً"
    //        );
    //    }

    //    return Ok();
    //}


    [HttpPost("AddTelegramAI")]
    public async Task<IActionResult> PostTelegramAI([FromBody] TelegramUpdate update)
    {
        
        if (update?.message == null || string.IsNullOrWhiteSpace(update.message.text) || update.message.from.is_bot)
            return Ok(); // تجاهل إذا ما في رسالة نصية

        var chatId = update.message.chat.id;
        var userMessage = update.message.text;


        if (_cache.TryGetValue(update.update_id, out _))
            return Ok();

        _cache.Set(update.update_id, true, TimeSpan.FromMinutes(5));
        try
        {
            await _mediator.Send(new SendTelegramAiBotCommand
            {
                message = userMessage,
                chatId = chatId
            });
        }
        catch (Exception ex)
        {
            // يمكنك تسجيل الخطأ هنا أيضاً
            await _botClient.SendMessage(
                chatId: chatId,
                text: "لا يوجد فكرة حاول مجدداً"
            );
        }
        return Ok();
    }



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



    [HttpGet("check")]
    public async Task<IActionResult> Check()
    {
        // جلب الـ API Key (من Environment Variable أو appsettings)
        var apiKey = _openAIConfig.ApiKey;
        if (string.IsNullOrEmpty(apiKey))
        {
            return BadRequest("❌ API Key not found in environment variables.");
        }

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // طلب بسيط لاختبار الاتصال
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = "Hello from Plesk test!" }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var body = await response.Content.ReadAsStringAsync();

        return Ok(new
        {
            StatusCode = response.StatusCode.ToString(),
            Response = body
        });
    }
}



//1955937367


//curl -X POST "https://api.telegram.org/bot7054212198:AAEfZRk1oVEqI5DeMLmFyL5q1dD-MNbkn80/setWebhook" \
//     -d "url=https://localhost:44390/api/webhook"

//    curl -X POST "https://api.telegram.org/bot7054212198:AAEfZRk1oVEqI5DeMLmFyL5q1dD-MNbkn80/setWebhook?url=https://localhost:44390/api/webhook"

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
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using FirstCall.Application.Models.Chat;

namespace FirstCall.Application.Features.ContactUs.Commands.AddEdit
{
    public partial class SendTelegramAiBotCommand : IRequest<Result<int>>
    {
        public long chatId { get; set; }
        public string message { get; set; }

    }

    internal class SendTelegramAiBotCommandHandler : IRequestHandler<SendTelegramAiBotCommand, Result<int>>
    {
        private readonly IMapper _mapper;

        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly MailSettings _mailSettings;
        private readonly ITelegramBotClient _botClient;

        public SendTelegramAiBotCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IOptions<MailSettings> mailSettings, ITelegramBotClient botClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _mailSettings = mailSettings.Value;
            _botClient = botClient;

        }


        public async Task<Result<int>> Handle(SendTelegramAiBotCommand command, CancellationToken cancellationToken)
        {

            await _botClient.SendMessage(
                chatId: command.chatId,
                text: command.message
            );

            return await Result<int>.SuccessAsync("Send Email Ok");

        }

        private async Task<string> GetAiResponse(string prompt)
        {
            // Example with dummy AI response
            await Task.Delay(100); // simulate processing
            return $"🤖 AI says: {prompt.ToUpper()}";
        }

    }
}

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
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using Org.BouncyCastle.Asn1.Crmf;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FirstCall.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using static FirstCall.Shared.Constants.Permission.Permissions;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Asn1.Ocsp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using Telegram.Bot.Types.ReplyMarkups;


namespace FirstCall.Application.Features.ContactUs.Commands.AddEdit
{
    public partial class SendTelegramAiBotByAICommandNew : IRequest<Result<int>>
    {
        public string naturalLanguageRequest { get; set; }
        public long chatId { get; set; }

    }

    internal class SendTelegramAiBotByAICommandNewHandler : IRequestHandler<SendTelegramAiBotByAICommandNew, Result<int>>
    {
        private readonly IMapper _mapper;

        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly MailSettings _mailSettings;
        private readonly ITelegramBotClient _botClient;
        private readonly HttpClient _httpClient;
        private readonly OpenAIConfig _openAIConfig;

        public SendTelegramAiBotByAICommandNewHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IOptions<MailSettings> mailSettings, ITelegramBotClient botClient, HttpClient httpClient, IOptions<OpenAIConfig> openAIConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _mailSettings = mailSettings.Value;
            _botClient = botClient;
            _httpClient = httpClient;
            _openAIConfig = openAIConfig.Value;
        }


        public async Task<Result<int>> Handle(SendTelegramAiBotByAICommandNew command, CancellationToken cancellationToken)
        {




            // لو ما فيه صورة، ممكن ترسل النص فقط


            await _botClient.SendMessage(
            chatId: command.chatId,
            text: "Hello");
            

            return await Result<int>.SuccessAsync("Send Email Ok");

        }


    }


}







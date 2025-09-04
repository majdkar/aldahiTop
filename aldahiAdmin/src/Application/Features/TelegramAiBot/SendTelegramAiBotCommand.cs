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
using System.Text.RegularExpressions;
using FirstCall.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using static FirstCall.Shared.Constants.Permission.Permissions;

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
            if (Regex.IsMatch(command.message, @"^[بB\s]+$") || command.message == "محير" ||  command.message == "Midlle" || command.message == "وسط" || command.message == "ولادي")
            {
                var products = await _unitOfWork.Repository<Product>().Entities
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Kind)
                  .Include(x => x.Season)
                  .Include(x => x.Group)
                  .Include(x => x.Warehouses)
                  .Where(x => x.Kind.NameAr.Contains(command.message) || x.Kind.NameEn.Contains(command.message)).ToListAsync();


                if (products.Any())
                {

                    foreach (var product in products)
                    {
                        string messageText =
                            $"📦 المنتج: {product.NameAr}\n" +
                            $"🔖 النوع: {product.Kind.NameAr}\n" +
                            $"🗂️ الصنف: {product.ProductCategory.NameAr}\n" +
                            $"🗂️ الكروب: {product.Group.NameAr}\n" +
                            $"🧩 الموسم: {product.Season.NameAr}\n" +
                            $"🔖 المستودع: {product.Warehouses.NameAr}\n" +
                            $"🔢 الكود: {product.Code}\n" +
                            $"💲 السعر: {product.Price}\n" +
                            $"📦 الكمية: {product.Qty}\n";


                        if (!string.IsNullOrEmpty(product.ProductImageUrl))
                        {
                            var photoUrl = "https://admin.aldahitop.com/" + product.ProductImageUrl;
                            await _botClient.SendPhoto(
                            chatId: command.chatId,
                            photo: photoUrl,
                            caption: messageText
                        );
                        }
                        else
                        {

                            await _botClient.SendMessage(
                            chatId: command.chatId,
                            text: messageText
                        );
                        }
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: command.chatId,
                        text: "❌ لا يوجد منتجات بهذا الصنف."
                    );
                }
            }


            if (command.message == "صيفي" || command.message == "شتوي" )
            {
                var products = await _unitOfWork.Repository<Product>().Entities
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Kind)
                  .Include(x => x.Season)
                  .Include(x => x.Group)
                  .Include(x => x.Warehouses)
                  .Where(x => x.Season.NameAr.Contains(command.message) || x.Season.NameEn.Contains(command.message)).ToListAsync();


                if (products.Any())
                {

                    foreach (var product in products)
                    {
                        string messageText =
                            $"📦 المنتج: {product.NameAr}\n" +
                            $"🔖 النوع: {product.Kind.NameAr}\n" +
                            $"🗂️ الصنف: {product.ProductCategory.NameAr}\n" +
                            $"🗂️ الكروب: {product.Group.NameAr}\n" +
                            $"🧩 الموسم: {product.Season.NameAr}\n" +
                            $"🔖 المستودع: {product.Warehouses.NameAr}\n" +
                            $"🔢 الكود: {product.Code}\n" +
                            $"💲 السعر: {product.Price}\n" +
                            $"📦 الكمية: {product.Qty}\n";


                        if (!string.IsNullOrEmpty(product.ProductImageUrl))
                        {
                            var photoUrl = "https://admin.aldahitop.com/" + product.ProductImageUrl;
                            await _botClient.SendPhoto(
                            chatId: command.chatId,
                            photo: photoUrl,
                            caption: messageText
                        );
                        }
                        else
                        {

                            await _botClient.SendMessage(
                            chatId: command.chatId,
                            text: messageText
                        );
                        }
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: command.chatId,
                        text: "❌ لا يوجد منتجات بهذا الموسم."
                    );
                }
            }

               
            if (command.message == "بناتي" || command.message == "صبياني" )
            {
                var products = await _unitOfWork.Repository<Product>().Entities
                  .Include(x => x.ProductCategory)
                  .Include(x => x.Kind)
                  .Include(x => x.Season)
                  .Include(x => x.Group)
                  .Include(x => x.Warehouses)
                  .Where(x => x.Group.NameAr.Contains(command.message) || x.Group.NameEn.Contains(command.message)).ToListAsync();


                if (products.Any())
                {

                    foreach (var product in products)
                    {
                        string messageText =
                            $"📦 المنتج: {product.NameAr}\n" +
                            $"🔖 النوع: {product.Kind.NameAr}\n" +
                            $"🗂️ الصنف: {product.ProductCategory.NameAr}\n" +
                            $"🗂️ الكروب: {product.Group.NameAr}\n" +
                            $"🧩 الموسم: {product.Season.NameAr}\n" +
                            $"🔖 المستودع: {product.Warehouses.NameAr}\n" +
                            $"🔢 الكود: {product.Code}\n" +
                            $"💲 السعر: {product.Price}\n" +
                            $"📦 الكمية: {product.Qty}\n";


                        if (!string.IsNullOrEmpty(product.ProductImageUrl))
                        {
                            var photoUrl = "https://admin.aldahitop.com/" + product.ProductImageUrl;
                            await _botClient.SendPhoto(
                            chatId: command.chatId,
                            photo: photoUrl,
                            caption: messageText
                        );
                        }
                        else
                        {

                            await _botClient.SendMessage(
                            chatId: command.chatId,
                            text: messageText
                        );
                        }
                    }
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: command.chatId,
                        text: "❌ لا يوجد منتجات بهذا المجموعة."
                    );
                }
            }


            if (Regex.IsMatch(command.message, @"^\d{1,5}$"))
            {
               var products = await _unitOfWork.Repository<Product>().Entities
                    .Include(x => x.ProductCategory)
                    .Include(x => x.Kind)
                    .Include(x => x.Season)
                    .Include(x => x.Group)
                    .Include(x => x.Warehouses)
                    .Where(x => x.Code == command.message).ToListAsync();





                if (products.Any())
                {
                    var first = products.First(); // بيانات عامة مشتركة
                    var kindsWithQty = string.Join("\n", products.Select(p => $"🔖 {p.Kind.NameAr} ➝ 📦 {p.Qty} ➝ 💲  {p.Price}"));

                    string messageText =
                        $"📦 المنتج: {first.NameAr}\n" +
                        $"🗂️ الصنف: {first.ProductCategory?.NameAr}\n" +
                        $"🗂️ الكروب: {first.Group?.NameAr}\n" +
                        $"🧩 الموسم: {first.Season?.NameAr}\n" +
                        $"🏬 المستودع: {first.Warehouses?.NameAr}\n" +
                        $"🔢 الكود: {first.Code}\n" +
                        $"📦 الكمية حسب النوع:\n{kindsWithQty}\n";

                    if (!string.IsNullOrEmpty(first.ProductImageUrl))
                    {
                        var photoUrl = "https://admin.aldahitop.com/" + first.ProductImageUrl;
                        await _botClient.SendPhoto(
                            chatId: command.chatId,
                            photo: photoUrl,
                            caption: messageText
                        );
                    }
                    else
                    {
                        await _botClient.SendMessage(
                            chatId: command.chatId,
                            text: messageText
                        );
                    }

                    //foreach (var product in products)
                    //{
                    //    string messageText =
                    //        $"📦 المنتج: {product.NameAr}\n" +
                    //        $"🔖 النوع: {product.Kind.NameAr}\n" +
                    //        $"🗂️ الصنف: {product.ProductCategory.NameAr}\n" +
                    //        $"🗂️ الكروب: {product.Group.NameAr}\n" +
                    //        $"🧩 الموسم: {product.Season.NameAr}\n" +
                    //        $"🔖 المستودع: {product.Warehouses.NameAr}\n" +
                    //        $"🔢 الكود: {product.Code}\n" +
                    //        $"💲 السعر: {product.Price}\n" +
                    //        $"📦 الكمية: {product.Qty}\n";


                    //    if (!string.IsNullOrEmpty(product.ProductImageUrl))
                    //    {
                    //        var photoUrl = "https://admin.aldahitop.com/" + product.ProductImageUrl;
                    //        await _botClient.SendPhoto(
                    //        chatId: command.chatId,
                    //        photo: photoUrl,
                    //        caption: messageText
                    //    );
                    //    }
                    //    else
                    //    {

                    //        await _botClient.SendMessage(
                    //        chatId: command.chatId,
                    //        text: messageText
                    //    );
                    //    }
                    //}
                }
                else
                {
                    await _botClient.SendMessage(
                        chatId: command.chatId,
                        text: "❌ لا يوجد منتجات بهذا الكود."
                    );
                }
            }
            
                    
                    
            return await Result<int>.SuccessAsync("Send Email Ok");

        }

    }
}

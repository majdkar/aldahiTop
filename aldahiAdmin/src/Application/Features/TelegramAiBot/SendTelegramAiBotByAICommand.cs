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
    public partial class SendTelegramAiBotByAICommand : IRequest<Result<int>>
    {
        public long chatId { get; set; }
        public string naturalLanguageRequest { get; set; }
        public string tableSchema { get; set; }

    }

    internal class SendTelegramAiBotByAICommandHandler : IRequestHandler<SendTelegramAiBotByAICommand, Result<int>>
    {
        private readonly IMapper _mapper;

        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly MailSettings _mailSettings;
        private readonly ITelegramBotClient _botClient;
        private readonly HttpClient _httpClient;

        public SendTelegramAiBotByAICommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IOptions<MailSettings> mailSettings, ITelegramBotClient botClient, HttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _mailSettings = mailSettings.Value;
            _botClient = botClient;
            _httpClient = httpClient;
        }


        public async Task<Result<int>> Handle(SendTelegramAiBotByAICommand command, CancellationToken cancellationToken)
        {



            //var prompt = $@"You are an SQL expert. Convert the following request into a safe SQL Server query.Only use the schema provided.Request: 
            //                {command.naturalLanguageRequest}
            //                Schema: {"TABLE[dbo].[Kinds]([Id][int] IDENTITY(1, 1) NOT NULL,[NameAr][nvarchar](max) NULL,[NameEn][nvarchar](max) NULL,[IsDeleted][bit] NOT NULL,"
            //                + "TABLE [dbo].[Categories](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[NameAr] [nvarchar](max) NULL,\r\n\t[NameEn] [nvarchar](max) NULL,\r\n\t[IsDeleted] [bit] NOT NULL,"
            //                + "TABLE [dbo].[Product](\\r\\n\\t[Id] [int] IDENTITY(1,1) NOT NULL,\\r\\n\\t[NameAr] [nvarchar](max) NULL,\\r\\n\\t[NameEn] [nvarchar](max) NULL,\\r\\n\\t[Sizes] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl4] [nvarchar](max) NULL,\\r\\n\\t[Code] [nvarchar](max) NULL,\\r\\n\\t[StorgePlace] [nvarchar](max) NULL,\\r\\n\\t[Price] [decimal](18, 2) NOT NULL,\\r\\n\\t[Order] [int] NOT NULL,\\r\\n\\t[ProductImageUrl] [nvarchar](max) NULL,\\r\\n\\t[IsDeleted] [bit] NOT NULL,\\r\\n\\t[CategoryId] [int] NOT NULL DEFAULT ((0)),\\r\\n\\t[Colors] [nvarchar](max) NULL,\\r\\n\\t[KindId] [int] NOT NULL DEFAULT ((0)),\\r\\n\\t[PackageNumber] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl2] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl3] [nvarchar](max) NULL,\\r\\n\\t[Qty] [int] NOT NULL DEFAULT ((0))\""}
            //                Do not include explanations, only the SQL statement
            //                In the database, the Product table contains a KindId column that links to Kinds.Id.
            //                 The Product table has a CategoryId column that links to the Categories table via Id.
            //                When querying products with type data, use an INNER JOIN between Product and Kinds with Product.KindId = Kinds.Id.
            //                use an INNER JOIN between Product and Categories with Product.CategoryId = Categories.Id.
            //                Be sure to filter out non-deleted records using IsDeleted = 0 in both tables.
            //              ";

            //var prompt = $@"You are an SQL expert. Convert the following request into a safe SQL Server query.Only use the schema provided.Request: 
            //                {command.naturalLanguageRequest}
            //                Schema: {"TABLE[dbo].[Kinds]([Id][int] IDENTITY(1, 1) NOT NULL,[NameAr][nvarchar](max) NULL,[NameEn][nvarchar](max) NULL,[IsDeleted][bit] NOT NULL,"
            //                + "TABLE [dbo].[Seasons](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[NameAr] [nvarchar](max) NULL,\r\n\t[NameEn] [nvarchar](max) NULL,\r\n\t[IsDeleted] [bit] NOT NULL,"
            //                + "TABLE [dbo].[Persons](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[ClientId] [int] NOT NULL,\r\n\t[CountryId] [int] NULL,\r\n\t[CityName] [nvarchar](max) NULL,\r\n\t[PersomImageUrl] [nvarchar](max) NULL,\r\n\t[FullName] [nvarchar](max) NULL,\r\n\t[Phone] [nvarchar](max) NULL,\r\n\t[Email] [nvarchar](max) NULL,\r\n\t[Address] [nvarchar](max) NULL,\r\n\t[AdditionalInfo] [nvarchar](max) NULL,\r\n\t[IsDeleted] [bit] NOT NULL,"
            //                + "TABLE [dbo].[Client](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[Type] [nvarchar](max) NULL,\r\n\t[Status] [nvarchar](max) NULL,\r\n\t[UserId] [nvarchar](450) NULL,\r\n\t[IsActive] [bit] NOT NULL,\r\n\t[IsDeleted] [bit] NOT NULL,"
            //                + "TABLE [dbo].[ProductCategories](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[NameAr] [nvarchar](max) NULL,\r\n\t[NameEn] [nvarchar](max) NULL,\r\n\t[IsDeleted] [bit] NOT NULL,"
            //                + "TABLE [dbo].[Product](\\r\\n\\t[Id] [int] IDENTITY(1,1) NOT NULL,\\r\\n\\t[NameAr] [nvarchar](max) NULL,\\r\\n\\t[NameEn] [nvarchar](max) NULL,\\r\\n\\t[Sizes] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl4] [nvarchar](max) NULL,\\r\\n\\t[Code] [nvarchar](max) NULL,\\r\\n\\t[StorgePlace] [nvarchar](max) NULL,\\r\\n\\t[Price] [decimal](18, 2) NOT NULL,\\r\\n\\t[Order] [int] NOT NULL,\\r\\n\\t[ProductImageUrl] [nvarchar](max) NULL,\\r\\n\\t[IsDeleted] [bit] NOT NULL,\\r\\n\\t[CategoryId] [int] NOT NULL DEFAULT ((0)),\\r\\n\\t[Colors] [nvarchar](max) NULL,\\r\\n\\t[KindId] [int] NOT NULL DEFAULT ((0)),\\r\\n\\t[PackageNumber] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl2] [nvarchar](max) NULL,\\r\\n\\t[ProductImageUrl3] [nvarchar](max) NULL,\\r\\n\\t[Qty] [int] NOT NULL DEFAULT ((0))\""}
            //                Do not include explanations, only the SQL statement
            //                In the database, 
            //                  the Product table contains a KindId column that links to Kinds.Id.
            //                 The Product table has a SeasonId column that links to the Seasons table via Id.
            //                 The Product table has a ProductCategoryId column that links to the ProductCategories table via Id.
                          
            //                When querying products with type data, use an INNER JOIN between Product and Kinds with Product.KindId = Kinds.Id.
            //                use an INNER JOIN between Product and Seasons with Product.SeasonId = Seasons.Id.
            //                use an INNER JOIN between Product and ProductCategories with Product.ProductCategoryId = ProductCategories.Id.

            //                When querying persons with type data,
            //                use an INNER JOIN between Persons and Client with Person.ClientId = Client.Id.

            //                Be sure to filter out non-deleted records using IsDeleted = 0 in both tables.
            //              ";


            var prompt = $@"You are an SQL Server expert. Convert the following natural language request into a safe SQL Server query using only the schema provided.

                                    Schema:

                                    Kinds(Id, NameAr, NameEn, IsDeleted)

                                    Groups(Id, NameAr, NameEn, IsDeleted)

                                    Seasons(Id, NameAr, NameEn, IsDeleted)

                                    Persons(Id, ClientId, CountryId, CityName, PersomImageUrl, FullName, Phone, Email, Address, AdditionalInfo, IsDeleted)

                                    Client(Id, Type, Status, UserId, IsActive, IsDeleted)

                                    ProductCategories(Id, NameAr, NameEn, IsDeleted)

                                    Product(Id, NameAr, NameEn, Sizes, ProductImageUrl,  Code, StorgePlace, Price, [Order], Qty, Colors, PackageNumber, IsDeleted, KindId, SeasonId, ProductCategoryId)

                                    Join rules:

                                    Product ↔ Kinds → Product.KindId = Kinds.Id

                                    Product ↔ Groups → Product.GroupId = Groups.Id

                                    Product ↔ Seasons → Product.SeasonId = Seasons.Id

                                    Product ↔ ProductCategories → Product.ProductCategoryId = ProductCategories.Id

                                    Persons ↔ Client → Persons.ClientId = Client.Id


                                    Semantic mapping:

                                    Clients, Customers, Users, العملاء, الزباين → Persons table.
                                    Girls, بناتي, صبياني, Guys → Groups table.


                                    Rules for string literals:
                                    - Always use N'...' for Arabic or Unicode text values.

                                    Constraints:

                                    Always filter out deleted rows → IsDeleted = 0 in all involved tables.

                                    Only return the SQL statement (no explanations).

                                    Request: {command.naturalLanguageRequest}
                          ";




            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
          {
                new { role = "user", content = prompt }
            }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonResponse);
            var completion = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            string fulltext = completion.Trim('`').StartsWith("sql") ? completion.Trim('`').Substring(4) : completion.Trim('`');

            //string cleanedQuery = ExtractSqlQuery(fulltext);



            var customers = await _unitOfWork.FromSqlRaw(fulltext);

            var input = command.naturalLanguageRequest?.ToLower() ?? "";

            bool isReportRequestXlsx =
                (input.Contains("تقرير") || input.Contains("report")) &&
                (input.Contains("xlsx")  || input.Contains("اكسل") || input.Contains("excel"));

            bool isReportRequestPdf =
                (input.Contains("تقرير") || input.Contains("report")) &&
                (input.Contains("pdf")  || input.Contains("بي دي اف"));

           if (isReportRequestXlsx)
            {
                // أنشئ ملف Excel جديد
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Products");

                // أضف رؤوس الأعمدة
                worksheet.Cell(1, 1).Value = "الكود";
                worksheet.Cell(1, 2).Value = "الاسم";
                worksheet.Cell(1, 3).Value = "السعر";
                worksheet.Cell(1, 4).Value = "الكمية";
                worksheet.Cell(1, 5).Value = "النوع";
                worksheet.Cell(1, 6).Value = "التصنيف";

                // املأ البيانات من قاعدة البيانات
                int rowIndex = 2;
                foreach (var product in customers)
                {
                    worksheet.Cell(rowIndex, 1).Value = product.TryGetValue("Code", out var code) ? code?.ToString() : "";
                    worksheet.Cell(rowIndex, 2).Value = product.TryGetValue("NameAr", out var nameAr) && nameAr != null
        ? nameAr.ToString()
        : product.TryGetValue("ProductNameAr", out var productNameAr) && productNameAr != null
            ? productNameAr.ToString()
            : ""; 
                    worksheet.Cell(rowIndex, 3).Value = product.TryGetValue("Price", out var price) ? price?.ToString() : "";
                    worksheet.Cell(rowIndex, 4).Value = product.TryGetValue("Qty", out var qty) ? qty?.ToString() : "";
                    worksheet.Cell(rowIndex, 5).Value = product.TryGetValue("KindNameAr", out var kind) ? kind?.ToString() : "";
                    worksheet.Cell(rowIndex, 6).Value = product.TryGetValue("CategoryNameAr", out var cat) ? cat?.ToString() : "";

                    rowIndex++;
                }

                // حفظ الملف في مجلد مؤقت
                var filePath = Path.Combine(Path.GetTempPath(), "ProductReport.xlsx");
                workbook.SaveAs(filePath);

                using var stream = File.OpenRead(filePath);
                await _botClient.SendDocument(
                    chatId: "1955937367",
                    document: new Telegram.Bot.Types.InputFileStream(stream, "ProductReport.xlsx"),
                    caption: "📊 تقرير المنتجات"
                );

            }

           else if (isReportRequestPdf)
            {
                 QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                 var filePath = Path.Combine(Path.GetTempPath(), "ProductReport.pdf");

                 QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(30);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Content().Table(table =>
                        {
                            // عدد الأعمدة
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // ID
                                columns.RelativeColumn(); // Name
                                columns.RelativeColumn(); // Price
                                columns.RelativeColumn(); // Qty
                                columns.RelativeColumn(); // Kind
                                columns.RelativeColumn(); // Category
                            });

                            // رؤوس الأعمدة
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("الكود");
                                header.Cell().Element(CellStyle).Text("الاسم");
                                header.Cell().Element(CellStyle).Text("السعر");
                                header.Cell().Element(CellStyle).Text("الكمية");
                                header.Cell().Element(CellStyle).Text("النوع");
                                header.Cell().Element(CellStyle).Text("التصنيف");
                            });

                            // البيانات
                            foreach (var product in customers)
                            {
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("Code", out var code) ? code?.ToString() : "");
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("NameAr", out var nameAr) && nameAr != null
        ? nameAr.ToString()
        : product.TryGetValue("ProductNameAr", out var productNameAr) && productNameAr != null
            ? productNameAr.ToString()
            : "");
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("Price", out var price) ? price?.ToString() : "");
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("Qty", out var qty) ? qty?.ToString() : "");
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("KindNameAr", out var kind) ? kind?.ToString() : "");
                                table.Cell().Element(CellStyle).Text(product.TryGetValue("CategoryNameAr", out var cat) ? cat?.ToString() : "");
                            }

                            IContainer CellStyle(IContainer container) =>
                                container.Padding(5).BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2);
                        });
                    });
                })
                .GeneratePdf(filePath);

                      using var pdfStream = File.OpenRead(filePath);

                await _botClient.SendDocument(
                    chatId: "1955937367",
                    document: new Telegram.Bot.Types.InputFileStream(pdfStream, "ProductReport.pdf"),
                    caption: "📄 تقرير المنتجات"
                );
            }

            else
            {
            foreach (var row in customers)
            {
                // الحصول على رابط الصورة من الـ dictionary (مثلاً مفتاح "ProductImageUrl")
                string photoUrl = row.ContainsKey("ProductImageUrl") && !string.IsNullOrWhiteSpace(row["ProductImageUrl"]?.ToString())
                    ? row["ProductImageUrl"].ToString()
                    : null;

                // بناء نص الرسالة لكل منتج
                var columnsText = row.Select(kv =>
                {
                    if (kv.Key == "ProductImageUrl") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "ProductImageUrl2") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "ProductImageUrl3") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "ProductImageUrl4") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "Id") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "ProductId") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "KindId") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "CategoryId") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "NameEn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "KindNameEn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "CategoryNameEn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "ProductNameEn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "CreatedBy") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "Order") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "CreatedOn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "LastModifiedBy") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "LastModifiedOn") return null; // لا نعرض الرابط نفسه في النص
                    if (kv.Key == "IsDeleted") return null; // لا نعرض الرابط نفسه في النص

                    string label = kv.Key switch
                    {
                        "NameAr" => "🧾 الاسم",
                        "ProductNameAr" => "🧾 الاسم",
                        "Price" => "💰 السعر",
                        "Sizes" => "📏 المقاسات",
                        "Qty" => "📦 الكمية",
                        "StorgePlace" => "🏬 مكان التخزين",
                        "PackageNumber" => "📦  رقم البكج",
                        "Code" => "🔢 كود",
                        "Colors" => "🎨 الألوان",
                        "Size" => "📏 المقاسات",
                        "KindNameAr" => "🧩 النوع",
                        "CategoryNameAr" => "🗂️ الصنف",
                        _ => $"🔸 {kv.Key}"
                    };

                    string value = string.IsNullOrWhiteSpace(kv.Value?.ToString()) ? "غير متوفر" : kv.Value.ToString();
                    return $"{label}: {value}";
                })
                .Where(text => text != null);

                string messageText = string.Join(Environment.NewLine, columnsText);


                    //var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    //  {
                    //new []
                    //{
                    //    InlineKeyboardButton.WithCallbackData("الطقس", "weather"),
                    //    InlineKeyboardButton.WithCallbackData("الأخبار", "news"),
                    //    InlineKeyboardButton.WithCallbackData("مساعدة", "help")
                    //}
                    // });


                    //var replyKeyboard = new ReplyKeyboardMarkup(new[]
                    //    {
                    //    new KeyboardButton[] { new KeyboardButton("الطقس"), new KeyboardButton("الأخبار"), new KeyboardButton("مساعدة") }
                    //})
                    //{
                    //    ResizeKeyboard = true,   // لتصغير حجم الكيبورد
                    //    OneTimeKeyboard = true   // لإخفاء الكيبورد بعد الضغط
                    //};

                    if (!string.IsNullOrEmpty(photoUrl))
                {
                    await _botClient.SendPhoto(
                        chatId: "1955937367",
                        photo: photoUrl,
                        caption: messageText
                    );
                }
                else
                {
                    // لو ما فيه صورة، ممكن ترسل النص فقط
                    //await _botClient.SendMessage(
                    //    chatId: "1955937367",
                    //    text: messageText, replyMarkup: replyKeyboard
                    //);
                        
                        await _botClient.SendMessage(
                        chatId: "1955937367",
                        text: messageText
                    );
                }
            }
            }



            return await Result<int>.SuccessAsync("Send Email Ok");

        }

        private async Task<string> GetAiResponse(string prompt)
        {
            // Example with dummy AI response
            await Task.Delay(100); // simulate processing
            return $"🤖 AI says: {prompt.ToUpper()}";
        }


        public static string ExtractSqlQuery(string input)
        {
            // أولاً نحاول نلتقط الجملة التي بين ```sql ... ```
            var codeBlockPattern = @"```sql\s*(.+?)\s*```";
            var codeBlockMatch = Regex.Match(input, codeBlockPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (codeBlockMatch.Success)
            {
                return codeBlockMatch.Groups[1].Value.Trim();
            }

            // fallback: نلتقط جملة تبدأ بـ SELECT أو غيره وتنتهي بفاصلة منقوطة
            var inlinePattern = @"(SELECT|INSERT|UPDATE|DELETE)[\s\S]*?;";
            var inlineMatch = Regex.Match(input, inlinePattern, RegexOptions.IgnoreCase);

            if (inlineMatch.Success)
            {
                return inlineMatch.Value.Trim();
            }

            return string.Empty;
        }

    }


}






//var prompt = $@"You are a professional SQL assistant.The database contains the following tables:
//                           schema : {"TABLE[dbo].[Kinds]([Id][int] IDENTITY(1, 1) NOT NULL,[NameAr][nvarchar](max) NULL,[NameEn][nvarchar](max) NULL,[IsDeleted][bit] NOT NULL," +
//                           "TABLE [dbo].[Product](\r\n\t[Id] [int] IDENTITY(1,1) NOT NULL,\r\n\t[NameAr] [nvarchar](max) NULL,\r\n\t[NameEn] [nvarchar](max) NULL,\r\n\t[Sizes] [nvarchar](max) NULL,\r\n\t[ProductImageUrl4] [nvarchar](max) NULL,\r\n\t[Code] [nvarchar](max) NULL,\r\n\t[StorgePlace] [nvarchar](max) NULL,\r\n\t[Price] [decimal](18, 2) NOT NULL,\r\n\t[Order] [int] NOT NULL,\r\n\t[ProductImageUrl] [nvarchar](max) NULL,\r\n\t[IsDeleted] [bit] NOT NULL,\r\n\t[CategoryId] [int] NOT NULL DEFAULT ((0)),\r\n\t[Colors] [nvarchar](max) NULL,\r\n\t[KindId] [int] NOT NULL DEFAULT ((0)),\r\n\t[PackageNumber] [nvarchar](max) NULL,\r\n\t[ProductImageUrl2] [nvarchar](max) NULL,\r\n\t[ProductImageUrl3] [nvarchar](max) NULL,\r\n\t[Qty] [int] NOT NULL DEFAULT ((0))"}
//                           Important Terms:
//                            - Absolutely no DELETE, UPDATE, DROP, or ALTER queries are allowed.
//                            - You should only respond with SELECT queries.
//                            - If the user requests something that requires a prohibited query, respond with a polite message like: ""Sorry, I can't create that type of query.""

//                            Now, create an SQL query based on the following request:
//                           {command.naturalLanguageRequest}";



//await _botClient.SendMessage(
//chatId: "1955937367",
//    text: string.Join(Environment.NewLine + "-----------------" + Environment.NewLine, customers.Select(c => $"{"الأسم :" + c.NameAr} {Environment.NewLine} {"السعر :" + c.Price} {Environment.NewLine} {"المقاسات :" + c.Sizes}  {Environment.NewLine} {"الكمية :" + c.Qty} "))
//);

//string messageText = string.Join(Environment.NewLine + "-----------------" + Environment.NewLine,
//    customers.Select(row =>
//    {
//        // لكل صف، نمر على كل عمود ونبني نص
//        var columnsText = row.Select(kv => $"{kv.Key} : {kv.Value ?? "غير متوفر"}");
//        return string.Join(Environment.NewLine, columnsText);
//    })
//    );

//            string messageText = string.Join(Environment.NewLine + "📍📍📍📍📍📍📍📍📍📍" + Environment.NewLine,
//    customers.Select(row =>
//    {
//        var columnsText = row.Select(kv =>
//        {
//            string label = kv.Key switch
//            {
//                "NameAr" => "🧾 الاسم",
//                "Price" => "💰 السعر",
//                "Sizes" => "📏 المقاسات",
//                "Qty" => "📦 الكمية",
//                _ => $"🔸 {kv.Key}"
//            };

//            string value = string.IsNullOrWhiteSpace(kv.Value?.ToString()) ? "غير متوفر" : kv.Value.ToString();
//            return $"{label}: {value}";
//        });

//        return string.Join(Environment.NewLine, columnsText);
//    })
//);




//            await _botClient.SendPhoto(
//                chatId: "1955937367",
//                caption: messageText ?? fulltext,
//                photo:                 );
// 


//string GetDictValue(Dictionary<string, object> dict, string key)
//{
//    if (dict.TryGetValue(key, out var value))
//        return value?.ToString() ?? "غير متوفر";
//    return "غير متوفر";
//}
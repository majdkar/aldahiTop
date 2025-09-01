using FirstCall.Application.Extensions;
using FirstCall.Application.Interfaces.Services;
using FirstCall.Application.Requests.Mail;
using FirstCall.Infrastructure.Extensions;
using FirstCall.Infrastructure.Services;
using FirstCall.Server.Extensions;
using FirstCall.Server.Filters;
using FirstCall.Server.Managers.Preferences;
using FirstCall.Server.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Net.Http;
using Telegram.Bot;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace FirstCall.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSignalR();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddCurrentUserService();
            services.AddSerialization();
            services.AddDatabase(_configuration);
            services.AddServerStorage(); //TODO - should implement ServerStorageProvider to work correctly!
            services.AddScoped<ServerPreferenceManager>();
            services.AddServerLocalization();
            services.AddIdentity();
            services.AddJwtAuthentication(services.GetApplicationSettings(_configuration));
            services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));
            services.Configure<OpenAIConfig>(_configuration.GetSection("OpenAI"));
           
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient("7054212198:AAEfZRk1oVEqI5DeMLmFyL5q1dD-MNbkn80"));

            services.AddHttpClient();


            //services.AddScoped<IMailingService, MailingService>();
            //services.AddSingleton<MailingService>();
            services.AddApplicationLayer();
            services.AddApplicationServices();
            services.AddRepositories();
            services.AddExtendedAttributesUnitOfWork();
            services.AddSharedInfrastructure(_configuration);
            services.RegisterSwagger();
            services.AddInfrastructureMappings();
            services.AddHangfire(x => x.UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddControllers().AddValidators();
            services.AddExtendedAttributesValidators();
            services.AddExtendedAttributesHandlers();
            services.AddRazorPages();
            services.AddTransient<IMailingService, MailingService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            //services.Configure<FormOptions>(config =>
            //{
            //    config.ValueLengthLimit = 5000; // Limit on individual form values
            //    config.MultipartBodyLengthLimit = 737280000; // Limit on form body size
            //    config.MultipartHeadersLengthLimit = 737280000; // Limit on form header size
            //});
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = 837280000; // Limit on request body size
            //});
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = null;
            //});
            //services.Configure<KestrelServerOptions>(options =>
            //{
            //    options.Limits.MaxRequestBodySize = null;
            //});
            services.AddLazyCache();
            // services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        public async void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IWebHostEnvironment env, IStringLocalizer<Startup> localizer)
        {

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseExceptionHandling(env);
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
                RequestPath = new PathString("/Files")
            });

            app.UseRequestLocalizationByCulture();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DashboardTitle = localizer["BlazorHero Jobs"],
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.UseEndpoints();
            app.ConfigureSwagger();
            await app.Initialize(serviceProvider);
        }
    }
}
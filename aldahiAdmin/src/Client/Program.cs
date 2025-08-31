using FirstCall.Client.Extensions;
using FirstCall.Client.Infrastructure.Managers.Preferences;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Client.Infrastructure.Settings;
using FirstCall.Shared.Constants.Localization;
using System.Net.Http;
using System;
using Append.Blazor.Printing;

namespace FirstCall.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            //var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //builder.RootComponents.Add<App>("app");

            //var url = builder.Configuration.GetValue<string>("ApiConfig:Url");
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(url) });



            var builder = WebAssemblyHostBuilder
                          .CreateDefault(args)
                          .AddRootComponents()
                          .AddClientServices();
            builder.Services.AddScoped<IPrintingService, PrintingService>();
            //builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://api.uaearchery.online/") });
            var host = builder.Build();
            var storageService = host.Services.GetRequiredService<ClientPreferenceManager>();
            if (storageService != null)
            {
                CultureInfo culture;
                var preference = await storageService.GetPreference() as ClientPreference;
                if (preference != null)
                    culture = new CultureInfo(preference.LanguageCode);
                else
                    culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "ar-SY");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            await builder.Build().RunAsync();
        }
    }
}
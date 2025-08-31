using System.Linq;
using FirstCall.Shared.Constants.Localization;
using FirstCall.Shared.Settings;

namespace FirstCall.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}
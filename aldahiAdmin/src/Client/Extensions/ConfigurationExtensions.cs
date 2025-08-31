using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FirstCall.Client.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Dictionary<string, string> GetCulturesSection(this IConfiguration configuration)
             => configuration.GetSection("Cultures")
                             .GetChildren()
                             .ToDictionary(k => k.Key, v => v.Value);
    }
}

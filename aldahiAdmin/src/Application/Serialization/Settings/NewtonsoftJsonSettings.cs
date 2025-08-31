
using FirstCall.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace FirstCall.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}
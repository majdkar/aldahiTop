using System.Text.Json;
using FirstCall.Application.Interfaces.Serialization.Options;

namespace FirstCall.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}
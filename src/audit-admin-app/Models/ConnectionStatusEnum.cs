using System.Text.Json.Serialization;

namespace Covario.ChatApp.models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ConnectionStatusEnum
    {
        Offline,
        Online
    }
}
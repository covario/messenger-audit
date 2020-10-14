using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TdLib
{
    /// <summary>
    /// Autogenerated TDLib APIs
    /// </summary>
    public static partial class TdApi
    {
        /// <summary>
        /// Returns an HTTP URL with the chat statistics. Currently this method can be used only for channels. Can be used only if SupergroupFullInfo.can_view_statistics == true 
        /// </summary>
        public class GetChatStatisticsUrl : Function<HttpUrl>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "getChatStatisticsUrl";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Chat identifier 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("chat_id")]
            public long ChatId { get; set; }

            /// <summary>
            /// Parameters from "tg://statsrefresh?params=******" link 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("parameters")]
            public string Parameters { get; set; }

            /// <summary>
            /// Pass true if a URL with the dark theme must be returned
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("is_dark")]
            public bool IsDark { get; set; }
        }


        /// <summary>
        /// Returns an HTTP URL with the chat statistics. Currently this method can be used only for channels. Can be used only if SupergroupFullInfo.can_view_statistics == true 
        /// </summary>
        public static Task<HttpUrl> GetChatStatisticsUrlAsync(this Client client,
            long chatId = default(long),
            string parameters = default(string),
            bool isDark = default(bool))
        {
            return client.ExecuteAsync(new GetChatStatisticsUrl
            {
                ChatId = chatId,
                Parameters = parameters,
                IsDark = isDark,
            });
        }
    }
}
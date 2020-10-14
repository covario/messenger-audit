using System;
using Newtonsoft.Json;

namespace TdLib
{
    /// <summary>
    /// Autogenerated TDLib APIs
    /// </summary>
    public static partial class TdApi
    {
        /// <summary>
        /// Contains information about one website the current user is logged in with Telegram
        /// </summary>
        public class ConnectedWebsite : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "connectedWebsite";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Website identifier
            /// </summary>
            [JsonConverter(typeof(Converter.Int64))]
            [JsonProperty("id")]
            public Int64 Id { get; set; }

            /// <summary>
            /// The domain name of the website
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("domain_name")]
            public string DomainName { get; set; }

            /// <summary>
            /// User identifier of a bot linked with the website
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("bot_user_id")]
            public int BotUserId { get; set; }

            /// <summary>
            /// The version of a browser used to log in
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("browser")]
            public string Browser { get; set; }

            /// <summary>
            /// Operating system the browser is running on
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("platform")]
            public string Platform { get; set; }

            /// <summary>
            /// Point in time (Unix timestamp) when the user was logged in
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("log_in_date")]
            public int LogInDate { get; set; }

            /// <summary>
            /// Point in time (Unix timestamp) when obtained authorization was last used
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("last_active_date")]
            public int LastActiveDate { get; set; }

            /// <summary>
            /// IP address from which the user was logged in, in human-readable format
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("ip")]
            public string Ip { get; set; }

            /// <summary>
            /// Human-readable description of a country and a region, from which the user was logged in, based on the IP address
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("location")]
            public string Location { get; set; }
        }
    }
}
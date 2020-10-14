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
        /// The current user has connected a website by logging in using Telegram Login Widget on it 
        /// </summary>
        public partial class MessageContent : Object
        {
            /// <summary>
            /// The current user has connected a website by logging in using Telegram Login Widget on it 
            /// </summary>
            public class MessageWebsiteConnected : MessageContent
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "messageWebsiteConnected";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Domain name of the connected website
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("domain_name")]
                public string DomainName { get; set; }
            }
        }
    }
}
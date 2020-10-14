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
        /// Contains a public HTTPS link to a message in a supergroup or channel with a username 
        /// </summary>
        public class PublicMessageLink : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "publicMessageLink";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Message link 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("link")]
            public string Link { get; set; }

            /// <summary>
            /// HTML-code for embedding the message
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("html")]
            public string Html { get; set; }
        }
    }
}
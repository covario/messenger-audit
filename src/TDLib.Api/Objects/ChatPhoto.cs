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
        /// Describes the photo of a chat 
        /// </summary>
        public class ChatPhoto : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "chatPhoto";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// A small (160x160) chat photo. The file can be downloaded only before the photo is changed 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("small")]
            public File Small { get; set; }

            /// <summary>
            /// A big (640x640) chat photo. The file can be downloaded only before the photo is changed
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("big")]
            public File Big { get; set; }
        }
    }
}
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
        /// A chat photo was changed 
        /// </summary>
        public partial class Update : Object
        {
            /// <summary>
            /// A chat photo was changed 
            /// </summary>
            public class UpdateChatPhoto : Update
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "updateChatPhoto";

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
                /// The new chat photo; may be null
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("photo")]
                public ChatPhoto Photo { get; set; }
            }
        }
    }
}
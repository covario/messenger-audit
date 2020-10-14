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
        /// Contains a chat invite link 
        /// </summary>
        public class ChatInviteLink : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "chatInviteLink";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Chat invite link
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("invite_link")]
            public string InviteLink { get; set; }
        }
    }
}
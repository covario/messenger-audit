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
        /// Returns users banned from the chat; can be used only by administrators in a supergroup or in a channel
        /// </summary>
        public partial class ChatMembersFilter : Object
        {
            /// <summary>
            /// Returns users banned from the chat; can be used only by administrators in a supergroup or in a channel
            /// </summary>
            public class ChatMembersFilterBanned : ChatMembersFilter
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "chatMembersFilterBanned";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }
            }
        }
    }
}
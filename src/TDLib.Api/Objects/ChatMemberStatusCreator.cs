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
        /// The user is the owner of a chat and has all the administrator privileges
        /// </summary>
        public partial class ChatMemberStatus : Object
        {
            /// <summary>
            /// The user is the owner of a chat and has all the administrator privileges
            /// </summary>
            public class ChatMemberStatusCreator : ChatMemberStatus
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "chatMemberStatusCreator";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// A custom title of the owner; 0-16 characters without emojis; applicable to supergroups only
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("custom_title")]
                public string CustomTitle { get; set; }

                /// <summary>
                /// True, if the user is a member of the chat
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("is_member")]
                public bool IsMember { get; set; }
            }
        }
    }
}
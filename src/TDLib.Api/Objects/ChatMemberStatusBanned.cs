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
        /// The user was banned (and hence is not a member of the chat). Implies the user can't return to the chat or view messages
        /// </summary>
        public partial class ChatMemberStatus : Object
        {
            /// <summary>
            /// The user was banned (and hence is not a member of the chat). Implies the user can't return to the chat or view messages
            /// </summary>
            public class ChatMemberStatusBanned : ChatMemberStatus
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "chatMemberStatusBanned";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Point in time (Unix timestamp) when the user will be unbanned; 0 if never. If the user is banned for more than 366 days or for less than 30 seconds from the current time, the user is considered to be banned forever
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("banned_until_date")]
                public int BannedUntilDate { get; set; }
            }
        }
    }
}
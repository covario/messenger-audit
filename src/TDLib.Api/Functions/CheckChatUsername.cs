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
        /// Checks whether a username can be set for a chat 
        /// </summary>
        public class CheckChatUsername : Function<CheckChatUsernameResult>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "checkChatUsername";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Chat identifier; should be identifier of a supergroup chat, or a channel chat, or a private chat with self, or zero if chat is being created 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("chat_id")]
            public long ChatId { get; set; }

            /// <summary>
            /// Username to be checked
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("username")]
            public string Username { get; set; }
        }


        /// <summary>
        /// Checks whether a username can be set for a chat 
        /// </summary>
        public static Task<CheckChatUsernameResult> CheckChatUsernameAsync(this Client client,
            long chatId = default(long),
            string username = default(string))
        {
            return client.ExecuteAsync(new CheckChatUsername
            {
                ChatId = chatId,
                Username = username,
            });
        }
    }
}
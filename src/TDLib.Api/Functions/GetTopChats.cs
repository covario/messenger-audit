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
        /// Returns a list of frequently used chats. Supported only if the chat info database is enabled 
        /// </summary>
        public class GetTopChats : Function<Chats>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "getTopChats";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Category of chats to be returned 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("category")]
            public TopChatCategory Category { get; set; }

            /// <summary>
            /// The maximum number of chats to be returned; up to 30
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("limit")]
            public int Limit { get; set; }
        }


        /// <summary>
        /// Returns a list of frequently used chats. Supported only if the chat info database is enabled 
        /// </summary>
        public static Task<Chats> GetTopChatsAsync(this Client client,
            TopChatCategory category = default(TopChatCategory),
            int limit = default(int))
        {
            return client.ExecuteAsync(new GetTopChats
            {
                Category = category,
                Limit = limit,
            });
        }
    }
}
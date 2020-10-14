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
        /// Returns a list of service actions taken by chat members and administrators in the last 48 hours. Available only for supergroups and channels. Requires administrator rights. Returns results in reverse chronological order (i. e., in order of decreasing event_id)
        /// </summary>
        public class GetChatEventLog : Function<ChatEvents>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "getChatEventLog";

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
            /// Search query by which to filter events 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("query")]
            public string Query { get; set; }

            /// <summary>
            /// Identifier of an event from which to return results. Use 0 to get results from the latest events 
            /// </summary>
            [JsonConverter(typeof(Converter.Int64))]
            [JsonProperty("from_event_id")]
            public Int64 FromEventId { get; set; }

            /// <summary>
            /// The maximum number of events to return; up to 100
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("limit")]
            public int Limit { get; set; }

            /// <summary>
            /// The types of events to return. By default, all types will be returned 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("filters")]
            public ChatEventLogFilters Filters { get; set; }

            /// <summary>
            /// User identifiers by which to filter events. By default, events relating to all users will be returned
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("user_ids")]
            public int[] UserIds { get; set; }
        }


        /// <summary>
        /// Returns a list of service actions taken by chat members and administrators in the last 48 hours. Available only for supergroups and channels. Requires administrator rights. Returns results in reverse chronological order (i. e., in order of decreasing event_id)
        /// </summary>
        public static Task<ChatEvents> GetChatEventLogAsync(this Client client,
            long chatId = default(long),
            string query = default(string),
            Int64 fromEventId = default(Int64),
            int limit = default(int),
            ChatEventLogFilters filters = default(ChatEventLogFilters),
            int[] userIds = default(int[]))
        {
            return client.ExecuteAsync(new GetChatEventLog
            {
                ChatId = chatId,
                Query = query,
                FromEventId = fromEventId,
                Limit = limit,
                Filters = filters,
                UserIds = userIds,
            });
        }
    }
}
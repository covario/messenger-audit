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
        /// Informs TDLib that messages are being viewed by the user. Many useful activities depend on whether the messages are currently being viewed or not (e.g., marking messages as read, incrementing a view counter, updating a view counter, removing deleted messages in supergroups and channels) 
        /// </summary>
        public class ViewMessages : Function<Ok>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "viewMessages";

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
            /// The identifiers of the messages being viewed
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("message_ids")]
            public long[] MessageIds { get; set; }

            /// <summary>
            /// True, if messages in closed chats should be marked as read
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("force_read")]
            public bool ForceRead { get; set; }
        }


        /// <summary>
        /// Informs TDLib that messages are being viewed by the user. Many useful activities depend on whether the messages are currently being viewed or not (e.g., marking messages as read, incrementing a view counter, updating a view counter, removing deleted messages in supergroups and channels) 
        /// </summary>
        public static Task<Ok> ViewMessagesAsync(this Client client,
            long chatId = default(long),
            long[] messageIds = default(long[]),
            bool forceRead = default(bool))
        {
            return client.ExecuteAsync(new ViewMessages
            {
                ChatId = chatId,
                MessageIds = messageIds,
                ForceRead = forceRead,
            });
        }
    }
}
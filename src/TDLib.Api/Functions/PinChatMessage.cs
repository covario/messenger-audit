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
        /// Pins a message in a chat; requires can_pin_messages rights 
        /// </summary>
        public class PinChatMessage : Function<Ok>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "pinChatMessage";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Identifier of the chat 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("chat_id")]
            public long ChatId { get; set; }

            /// <summary>
            /// Identifier of the new pinned message 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("message_id")]
            public long MessageId { get; set; }

            /// <summary>
            /// True, if there should be no notification about the pinned message
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("disable_notification")]
            public bool DisableNotification { get; set; }
        }


        /// <summary>
        /// Pins a message in a chat; requires can_pin_messages rights 
        /// </summary>
        public static Task<Ok> PinChatMessageAsync(this Client client,
            long chatId = default(long),
            long messageId = default(long),
            bool disableNotification = default(bool))
        {
            return client.ExecuteAsync(new PinChatMessage
            {
                ChatId = chatId,
                MessageId = messageId,
                DisableNotification = disableNotification,
            });
        }
    }
}
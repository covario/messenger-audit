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
        /// Returns information about a successful payment 
        /// </summary>
        public class GetPaymentReceipt : Function<PaymentReceipt>
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "getPaymentReceipt";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Chat identifier of the PaymentSuccessful message 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("chat_id")]
            public long ChatId { get; set; }

            /// <summary>
            /// Message identifier
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("message_id")]
            public long MessageId { get; set; }
        }


        /// <summary>
        /// Returns information about a successful payment 
        /// </summary>
        public static Task<PaymentReceipt> GetPaymentReceiptAsync(this Client client,
            long chatId = default(long),
            long messageId = default(long))
        {
            return client.ExecuteAsync(new GetPaymentReceipt
            {
                ChatId = chatId,
                MessageId = messageId,
            });
        }
    }
}
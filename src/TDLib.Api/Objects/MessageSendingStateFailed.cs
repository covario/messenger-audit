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
        /// The message failed to be sent 
        /// </summary>
        public partial class MessageSendingState : Object
        {
            /// <summary>
            /// The message failed to be sent 
            /// </summary>
            public class MessageSendingStateFailed : MessageSendingState
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "messageSendingStateFailed";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// An error code; 0 if unknown 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("error_code")]
                public int ErrorCode { get; set; }

                /// <summary>
                /// Error message
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("error_message")]
                public string ErrorMessage { get; set; }

                /// <summary>
                /// True, if the message can be re-sent 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("can_retry")]
                public bool CanRetry { get; set; }

                /// <summary>
                /// Time left before the message can be re-sent, in seconds. No update is sent when this field changes
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("retry_after")]
                public double? RetryAfter { get; set; }
            }
        }
    }
}
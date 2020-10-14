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
        /// A basic group was upgraded to a supergroup and was deactivated as the result 
        /// </summary>
        public partial class MessageContent : Object
        {
            /// <summary>
            /// A basic group was upgraded to a supergroup and was deactivated as the result 
            /// </summary>
            public class MessageChatUpgradeTo : MessageContent
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "messageChatUpgradeTo";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Identifier of the supergroup to which the basic group was upgraded
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("supergroup_id")]
                public int SupergroupId { get; set; }
            }
        }
    }
}
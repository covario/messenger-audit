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
        /// Represents an audio file 
        /// </summary>
        public partial class InlineQueryResult : Object
        {
            /// <summary>
            /// Represents an audio file 
            /// </summary>
            public class InlineQueryResultAudio : InlineQueryResult
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "inlineQueryResultAudio";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Unique identifier of the query result 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("id")]
                public string Id { get; set; }

                /// <summary>
                /// Audio file
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("audio")]
                public Audio Audio { get; set; }
            }
        }
    }
}
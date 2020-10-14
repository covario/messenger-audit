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
        /// A simple object containing a sequence of bytes; for testing only 
        /// </summary>
        public class TestBytes : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "testBytes";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// Bytes
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("value")]
            public byte[] Value { get; set; }
        }
    }
}
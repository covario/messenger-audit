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
        /// A text with some entities 
        /// </summary>
        public class FormattedText : Object
        {
            /// <summary>
            /// Data type for serialization
            /// </summary>
            [JsonProperty("@type")]
            public override string DataType { get; set; } = "formattedText";

            /// <summary>
            /// Extra data attached to the message
            /// </summary>
            [JsonProperty("@extra")]
            public override string Extra { get; set; }

            /// <summary>
            /// The text 
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("text")]
            public string Text { get; set; }

            /// <summary>
            /// Entities contained in the text. Entities can be nested, but must not mutually intersect with each other.
            /// </summary>
            [JsonConverter(typeof(Converter))]
            [JsonProperty("entities")]
            public TextEntity[] Entities { get; set; }
        }
    }
}
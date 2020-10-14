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
        /// Represents information about a venue 
        /// </summary>
        public partial class InlineQueryResult : Object
        {
            /// <summary>
            /// Represents information about a venue 
            /// </summary>
            public class InlineQueryResultVenue : InlineQueryResult
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "inlineQueryResultVenue";

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
                /// Venue result 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("venue")]
                public Venue Venue { get; set; }

                /// <summary>
                /// Result thumbnail; may be null
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("thumbnail")]
                public PhotoSize Thumbnail { get; set; }
            }
        }
    }
}
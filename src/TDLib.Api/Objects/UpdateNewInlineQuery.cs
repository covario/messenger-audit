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
        /// A new incoming inline query; for bots only 
        /// </summary>
        public partial class Update : Object
        {
            /// <summary>
            /// A new incoming inline query; for bots only 
            /// </summary>
            public class UpdateNewInlineQuery : Update
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "updateNewInlineQuery";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Unique query identifier 
                /// </summary>
                [JsonConverter(typeof(Converter.Int64))]
                [JsonProperty("id")]
                public Int64 Id { get; set; }

                /// <summary>
                /// Identifier of the user who sent the query 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("sender_user_id")]
                public int SenderUserId { get; set; }

                /// <summary>
                /// User location, provided by the client; may be null
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("user_location")]
                public Location UserLocation { get; set; }

                /// <summary>
                /// Text of the query 
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("query")]
                public string Query { get; set; }

                /// <summary>
                /// Offset of the first entry to return
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("offset")]
                public string Offset { get; set; }
            }
        }
    }
}
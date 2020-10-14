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
        /// An invisible anchor on a page, which can be used in a URL to open the page from the specified anchor 
        /// </summary>
        public partial class PageBlock : Object
        {
            /// <summary>
            /// An invisible anchor on a page, which can be used in a URL to open the page from the specified anchor 
            /// </summary>
            public class PageBlockAnchor : PageBlock
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "pageBlockAnchor";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Name of the anchor
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("name")]
                public string Name { get; set; }
            }
        }
    }
}
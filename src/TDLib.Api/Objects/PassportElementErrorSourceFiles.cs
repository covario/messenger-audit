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
        /// The list of attached files contains an error. The error will be considered resolved when the list of files changes
        /// </summary>
        public partial class PassportElementErrorSource : Object
        {
            /// <summary>
            /// The list of attached files contains an error. The error will be considered resolved when the list of files changes
            /// </summary>
            public class PassportElementErrorSourceFiles : PassportElementErrorSource
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "passportElementErrorSourceFiles";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }
            }
        }
    }
}
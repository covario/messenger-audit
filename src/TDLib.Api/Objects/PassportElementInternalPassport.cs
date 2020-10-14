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
        /// A Telegram Passport element containing the user's internal passport 
        /// </summary>
        public partial class PassportElement : Object
        {
            /// <summary>
            /// A Telegram Passport element containing the user's internal passport 
            /// </summary>
            public class PassportElementInternalPassport : PassportElement
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "passportElementInternalPassport";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }

                /// <summary>
                /// Internal passport
                /// </summary>
                [JsonConverter(typeof(Converter))]
                [JsonProperty("internal_passport")]
                public IdentityDocument InternalPassport { get; set; }
            }
        }
    }
}
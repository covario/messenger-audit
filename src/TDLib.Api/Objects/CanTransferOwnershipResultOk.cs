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
        /// The session can be used
        /// </summary>
        public partial class CanTransferOwnershipResult : Object
        {
            /// <summary>
            /// The session can be used
            /// </summary>
            public class CanTransferOwnershipResultOk : CanTransferOwnershipResult
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "canTransferOwnershipResultOk";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }
            }
        }
    }
}
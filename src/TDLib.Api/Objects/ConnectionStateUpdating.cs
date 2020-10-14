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
        /// Downloading data received while the client was offline
        /// </summary>
        public partial class ConnectionState : Object
        {
            /// <summary>
            /// Downloading data received while the client was offline
            /// </summary>
            public class ConnectionStateUpdating : ConnectionState
            {
                /// <summary>
                /// Data type for serialization
                /// </summary>
                [JsonProperty("@type")]
                public override string DataType { get; set; } = "connectionStateUpdating";

                /// <summary>
                /// Extra data attached to the message
                /// </summary>
                [JsonProperty("@extra")]
                public override string Extra { get; set; }
            }
        }
    }
}